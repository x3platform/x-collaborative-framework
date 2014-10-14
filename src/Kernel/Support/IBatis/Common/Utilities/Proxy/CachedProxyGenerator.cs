
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 398108 $
 * $Date: 2006-04-29 11:39:42 +0200 (sam., 29 avr. 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

#region Using

using System;
using System.Collections;
using System.Collections.Specialized;
using X3Platform.DynamicProxy;
using X3Platform.IBatis.Common.Exceptions;
using X3Platform.IBatis.Common.Logging;
using X3Platform.Logging;

#endregion

namespace X3Platform.IBatis.Common.Utilities.Proxy
{
    /// <summary>
    /// An ProxyGenerator with cache that uses the X3Platform.DynamicProxy library.
    /// </summary>
    [CLSCompliant(false)]
    public class CachedProxyGenerator : ProxyGenerator
    {
        private static readonly IInternalLog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // key = mapped type
        // value = proxy type
        private IDictionary _cachedProxyTypes = null;

        /// <summary>
        /// Cosntructor
        /// </summary>
        public CachedProxyGenerator()
        {
            _cachedProxyTypes = new HybridDictionary();
        }

        /// <summary>
        /// Generates a proxy implementing all the specified interfaces and
        /// redirecting method invocations to the specifed interceptor.
        /// </summary>
        /// <param name="theInterface">Interface to be implemented</param>
        /// <param name="interceptor">instance of <see cref="IInterceptor"/></param>
        /// <param name="target">The target object.</param>
        /// <returns>Proxy instance</returns>
        public override object CreateProxy(Type theInterface, IInterceptor interceptor, object target)
        {
            return CreateProxy(new Type[] { theInterface }, interceptor, target);
        }

        /// <summary>
        /// Generates a proxy implementing all the specified interfaces and
        /// redirecting method invocations to the specifed interceptor.
        /// </summary>
        /// <param name="interfaces">Array of interfaces to be implemented</param>
        /// <param name="interceptor">instance of <see cref="IInterceptor"/></param>
        /// <param name="target">The target object.</param>
        /// <returns>Proxy instance</returns>
        public override object CreateProxy(Type[] interfaces, IInterceptor interceptor, object target)
        {
            try
            {
                System.Type proxyType = null;
                System.Type targetType = target.GetType();

                lock (_cachedProxyTypes.SyncRoot)
                {
                    proxyType = _cachedProxyTypes[targetType] as System.Type;

                    if (proxyType == null)
                    {
                        proxyType = ProxyBuilder.CreateInterfaceProxy(interfaces, targetType);
                        _cachedProxyTypes[targetType] = proxyType;
                    }
                }
                return base.CreateProxyInstance(proxyType, interceptor, target);
            }
            catch (Exception e)
            {
                _log.Error("Castle Dynamic Proxy Generator failed", e);
                throw new IBatisException("Castle Proxy Generator failed", e);
            }
        }



        /// <summary>
        /// Generates a proxy implementing all the specified interfaces and
        /// redirecting method invocations to the specifed interceptor.
        /// This proxy is for object different from IList or ICollection
        /// </summary>
        /// <param name="targetType">The target type</param>
        /// <param name="interceptor">The interceptor.</param>
        /// <param name="argumentsForConstructor">The arguments for constructor.</param>
        /// <returns></returns>
        public override object CreateClassProxy(Type targetType, IInterceptor interceptor, params object[] argumentsForConstructor)
        {
            try
            {
                System.Type proxyType = null;

                lock (_cachedProxyTypes.SyncRoot)
                {
                    proxyType = _cachedProxyTypes[targetType] as System.Type;

                    if (proxyType == null)
                    {
                        proxyType = ProxyBuilder.CreateClassProxy(targetType);
                        _cachedProxyTypes[targetType] = proxyType;
                    }
                }
                return CreateClassProxyInstance(proxyType, interceptor, argumentsForConstructor);
            }
            catch (Exception e)
            {
                _log.Error("Castle Dynamic Class-Proxy Generator failed", e);
                throw new IBatisException("Castle Proxy Generator failed", e);
            }

        }
    }
}
