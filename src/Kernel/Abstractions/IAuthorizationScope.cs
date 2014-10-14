// =============================================================================
//
// Copyright (c) x3platfrom.com
//
// FileName     :IAuthorizationScope.cs
//
// Description  :Ȩ�޷�Χ�ӿ�
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================

using System;
using System.Collections.Generic;

using X3Platform.Security.Authority;

namespace X3Platform
{
    /// <summary>Ȩ�޷�Χ�ӿ�</summary>
    public interface IAuthorizationScope
    {
        /// <summary>Ȩ�޶�����Ϣ</summary>
        IAuthorizationObject AuthorizationObject { get; set; }

        /// <summary>Ȩ����Ϣ</summary>
        AuthorityInfo Authority { get; set; }
        
        /// <summary>ʵ����Ϣ</summary>
        EntityClass EntityClass { get; set; }
    }
}
