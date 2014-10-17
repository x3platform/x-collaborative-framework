using System;
using System.Collections.Generic;

using X3Platform.Security.Authority;

namespace X3Platform
{
    /// <summary>权限范围接口</summary>
    public interface IAuthorizationScope
    {
        /// <summary>权限对象信息</summary>
        IAuthorizationObject AuthorizationObject { get; set; }

        /// <summary>权限信息</summary>
        AuthorityInfo Authority { get; set; }
        
        /// <summary>实体信息</summary>
        EntityClass EntityClass { get; set; }
    }
}
