namespace X3Platform.Membership.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    using X3Platform.Ajax;
    using X3Platform.Util;

    using X3Platform.Membership.IBLL;
    using X3Platform.Membership.Model;

    /// <summary>��ϵ������</summary>
    [Flags]
    public enum ContactType
    {
        /// <summary>Ĭ��</summary>
        Default = Account | OrganizationUnit | Role | Group,

        /// <summary>����</summary>
        All = Account | OrganizationUnit | Role | Group | StandardOrganizationUnit | StandardRole | GeneralRole | Contact,

        /// <summary>�ʺ�</summary>
        Account = 1,

        /// <summary>��֯��λ</summary>
        OrganizationUnit = 2,

        /// <summary>��ɫ</summary>
        Role = 4,

        /// <summary>Ⱥ��</summary>
        Group = 8,

        /// <summary>��׼��֯</summary>
        StandardOrganizationUnit = 16,

        /// <summary>��׼��ɫ</summary>
        StandardRole = 32,

        /// <summary>ͨ�ý�ɫ</summary>
        GeneralRole = 64,

        /// <summary>ְλ��Ϣ</summary>
        Job = 10240,

        /// <summary>��λ��Ϣ</summary>
        AssignedJob = 20480,

        /// <summary>�ҵ���ϵ��</summary>
        Contact = 65536,
    }
}