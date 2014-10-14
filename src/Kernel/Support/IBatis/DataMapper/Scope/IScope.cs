using X3Platform.IBatis.Common.Utilities.Objects;
using X3Platform.IBatis.Common.Utilities.Objects.Members;
using X3Platform.IBatis.DataMapper.DataExchange;
using X3Platform.IBatis.DataMapper.TypeHandlers;

#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
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

namespace X3Platform.IBatis.DataMapper.Scope
{
	/// <summary>
	///
	/// </summary>
	public interface IScope
	{

		/// <summary>
		///  Get the error context
		/// </summary>
		ErrorContext ErrorContext { get; }

		/// <summary>
		/// The factory for DataExchange objects
		/// </summary>
		DataExchangeFactory DataExchangeFactory { get; }
	}
}
