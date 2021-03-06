
#region Apache Notice
/*****************************************************************************
 * $Revision: 474910 $
 * $LastChangedDate: 2006-11-19 17:07:45 +0100 (dim., 19 nov. 2006) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006/2005 - The Apache Software Foundation
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
using System.Data;
using System.Globalization;

using X3Platform.IBatis.DataMapper.Configuration.ResultMapping;
#endregion

namespace X3Platform.IBatis.DataMapper.TypeHandlers.Customize
{
    /// <summary>将数据库中的ntext类型时间数据转化为string类型</summary>
    public class NTextStringTypeHandler : ITypeHandlerCallback
    {
        /// <summary>
        /// Performs processing on a value before it is used to set
        /// the parameter of a IDbCommand.
        /// </summary>
        /// <param name="setter">The interface for setting the value on the IDbCommand.</param>
        /// <param name="parameter">The value to be set</param>
        public void SetParameter(IParameterSetter setter, object parameter)
        {
            if (parameter == null)
            {
                setter.Value = string.Empty;
            }
            else
            {
                // 设置parameter的其它属性,本例为size.

                if (setter.DataParameter is System.Data.SqlClient.SqlParameter)
                {
                    // SQL Server
                    ((System.Data.SqlClient.SqlParameter)setter.DataParameter).Size = int.MaxValue;
                }

                setter.Value = parameter;
            }
        }

        /// <summary>
        /// Performs processing on a value before after it has been retrieved
        /// from a IDataReader.
        /// </summary>
        /// <param name="getter">The interface for getting the value from the IDataReader.</param>
        /// <returns>The processed value.</returns>
        public object GetResult(IResultGetter getter)
        {
            if (getter != null)
                return getter.Value.ToString();
            return string.Empty;
        }

        /// <summary>
        /// Casts the string representation of a value into a type recognized by
        /// this type handler.  This method is used to translate nullValue values
        /// into types that can be appropriately compared.  If your custom type handler
        /// cannot support nullValues, or if there is no reasonable string representation
        /// for this type (e.g. File type), you can simply return the String representation
        /// as it was passed in.  It is not recommended to return null, unless null was passed
        /// in.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public object ValueOf(string s)
        {
            return s;
        }

        /// <summary>空值</summary>
        public object NullValue
        {
            get { return null; }
        }
    }
}

