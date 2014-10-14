//=============================================================================
//
// Copyright (c) 2009 X3Platform
//
// Filename     :PagingHelper.cs
//
// Summary      :pages helper
//
// Author       :X3Platform
//
// Date			:2008-04-26
//
//=============================================================================


using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using X3Platform.Ajax;
using X3Platform.Util;

namespace X3Platform.Tests.Utility
{
    /// <summary>分页管理器</summary>
    [TestClass]
    public class XmlHelperTestSuite
    {
        [TestMethod]
        public void TestToXml()
        {
            string outString = "{\"ajaxStorage\":["
                //string outString = "{\"ajaxStorage\":[{\"id\":\"25\", \"date\":\"2009-05-26 09:55:20\", \"thread\":\"4352\", \"level\":\"INFO\", \"message\":\"2009-05-26 09:55:20 会员[测试员2(test)]登录了系统.[IP:127.0.0.1]\", \"exception\":\"System.Exception\" },"
                + "{\"id\":\"24\", \"date\":\"2009-05-26 09:43:48\", \"thread\":\"4352\", \"level\":\"ERROR\", \"message\":\"Cannot resolve type [X3Platform.Membership.Authentication.HttpAuthenticationManagement] for object with name 'X3Platform.Security.Authentication.IAuthenticationManagement' defined in file [E:\\Workspace\\X3Platform\\trunk\\WebSite\\1.0.0\\config\\X3Platform.Spring.Objects.config] line 13\", \"exception\":\"System.Exception: Spring.Core.CannotLoadObjectTypeException: Cannot resolve type [X3Platform.Membership.Authentication.HttpAuthenticationManagement] for object with name 'X3Platform.Security.Authentication.IAuthenticationManagement' defined in file [E:\\Workspace\\X3Platform\\trunk\\WebSite\\1.0.0\\config\\X3Platform.Spring.Objects.config] line 13 ---> System.TypeLoadException: Could not load type from string value 'X3Platform.Membership.Authentication.HttpAuthenticationManagement'.\u000d\u000a   at Spring.Core.TypeResolution.TypeResolver.Resolve(String typeName)\u000d\u000a   at Spring.Core.TypeResolution.GenericTypeResolver.Resolve(String typeName)\u000d\u000a   at Spring.Core.TypeResolution.CachedTypeResolver.Resolve(String typeName)\u000d\u000a   at Spring.Core.TypeResolution.TypeResolutionUtils.ResolveType(String typeName)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectDefinition.ResolveObjectType()\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.ResolveObjectType(RootObjectDefinition rod, String objectName)\u000d\u000a   --- End of inner exception stack trace ---\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.ResolveObjectType(RootObjectDefinition rod, String objectName)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractAutowireCapableObjectFactory.InstantiateObject(String name, RootObjectDefinition definition, Object[] arguments, Boolean allowEagerCaching, Boolean suppressConfigure)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.CreateAndCacheSingletonInstance(String objectName, RootObjectDefinition objectDefinition, Object[] arguments)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.GetObjectInternal(String name, Type requiredType, Object[] arguments, Boolean suppressConfigure)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.GetObject(String name)\u000d\u000a   at Spring.Context.Support.AbstractApplicationContext.GetObject(String name)\u000d\u000a   at X3Platform.Spring.SpringContext.GetObject[T](Type type) in E:\\Workspace\\X3Platform\\trunk\\Kernel\\Kernel\\Spring\\SpringContext.cs:line 126\u000d\u000a   at X3Platform.KernelContext.Reload() in E:\\Workspace\\X3Platform\\trunk\\Kernel\\Kernel\\KernelContext.cs:line 178\u000d\u000a\" },"
                //+ "{"id":"23", "date":"2009-05-26 09:43:48", "thread":"4352", "level":"ERROR", "message":"Cannot resolve type [X3Platform.Membership.Authentication.HttpAuthenticationManagement] for object with name 'X3Platform.Security.Authentication.IAuthenticationManagement' defined in file [E:\\Workspace\\X3Platform\\trunk\\WebSite\\1.0.0\\config\\X3Platform.Spring.Objects.config] line 13", "exception":"System.Exception: Spring.Core.CannotLoadObjectTypeException: Cannot resolve type [X3Platform.Membership.Authentication.HttpAuthenticationManagement] for object with name 'X3Platform.Security.Authentication.IAuthenticationManagement' defined in file [E:\\Workspace\\X3Platform\\trunk\\WebSite\\1.0.0\\config\\X3Platform.Spring.Objects.config] line 13 ---> System.TypeLoadException: Could not load type from string value 'X3Platform.Membership.Authentication.HttpAuthenticationManagement'.\u000d\u000a   at Spring.Core.TypeResolution.TypeResolver.Resolve(String typeName)\u000d\u000a   at Spring.Core.TypeResolution.GenericTypeResolver.Resolve(String typeName)\u000d\u000a   at Spring.Core.TypeResolution.CachedTypeResolver.Resolve(String typeName)\u000d\u000a   at Spring.Core.TypeResolution.TypeResolutionUtils.ResolveType(String typeName)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectDefinition.ResolveObjectType()\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.ResolveObjectType(RootObjectDefinition rod, String objectName)\u000d\u000a   --- End of inner exception stack trace ---\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.ResolveObjectType(RootObjectDefinition rod, String objectName)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractAutowireCapableObjectFactory.InstantiateObject(String name, RootObjectDefinition definition, Object[] arguments, Boolean allowEagerCaching, Boolean suppressConfigure)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.CreateAndCacheSingletonInstance(String objectName, RootObjectDefinition objectDefinition, Object[] arguments)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.GetObjectInternal(String name, Type requiredType, Object[] arguments, Boolean suppressConfigure)\u000d\u000a   at Spring.Objects.Factory.Support.AbstractObjectFactory.GetObject(String name)\u000d\u000a   at Spring.Context.Support.AbstractApplicationContext.GetObject(String name)\u000d\u000a   at X3Platform.Spring.SpringContext.GetObject[T](Type type) in E:\\Workspace\\X3Platform\\trunk\\Kernel\\Kernel\\Spring\\SpringContext.cs:line 117\u000d\u000a\" },"
                //+ "{\"id\":\"22\", \"date\":\"2009-05-25 14:51:07\", \"thread\":\"3168\", \"level\":\"INFO\", \"message\":\"2009-05-25 14:51:07 会员[测试员2(test)]登录了系统.[IP:]\", \"exception\":\"System.Exception\" },{\"id\":\"21\", \"date\":\"2009-05-25 14:50:59\", \"thread\":\"5216\", \"level\":\"INFO\", \"message\":\"2009-05-25 14:50:59 会员[测试员2(test)]登录了系统.[IP:]\", \"exception\":\"System.Exception\" },{\"id\":\"20\", \"date\":\"2009-05-25 14:50:46\", \"thread\":\"3168\", \"level\":\"INFO\", \"message\":\"2009-05-25 14:50:46 会员[测试员2(test)]登录了系统.[IP:]\", \"exception\":\"System.Exception\" },"
                + "{\"id\":\"19\", \"date\":\"2009-05-25 14:46:43\", \"thread\":\"5216\", \"level\":\"INFO\", \"message\":\"2009-05-25 14:46:43 会员[测试员2(test)]登录了系统.[IP:]\", \"exception\":\"System.Exception\" }]}";

            string result = XmlHelper.ToXml(outString);

        }
    }
}