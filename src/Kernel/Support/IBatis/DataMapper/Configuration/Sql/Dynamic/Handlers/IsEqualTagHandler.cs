
#region Apache Notice
/*****************************************************************************
 * $Revision: 405046 $
 * $LastChangedDate: 2006-05-08 15:21:44 +0200 (lun., 08 mai 2006) $
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

#region Imports
using System;
using X3Platform.IBatis.Common.Utilities.Objects.Members;
using X3Platform.IBatis.DataMapper.Configuration.Sql.Dynamic.Elements;
#endregion


namespace X3Platform.IBatis.DataMapper.Configuration.Sql.Dynamic.Handlers
{
	/// <summary>
	/// Summary description for IsEqualTagHandler.
	/// </summary>
	public class IsEqualTagHandler : ConditionalTagHandler
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="IsEqualTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public IsEqualTagHandler(AccessorFactory accessorFactory)
            : base(accessorFactory)
		{
		}

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="tag"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public override bool IsCondition(SqlTagContext ctx, SqlTag tag, object parameterObject)
		{
			return (this.Compare(ctx, tag, parameterObject) == 0);
		}
		#endregion

	}
}
