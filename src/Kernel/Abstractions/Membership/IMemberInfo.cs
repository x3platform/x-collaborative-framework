#region Copyright & Author
// =============================================================================
//
// Copyright (c) 2010 Elane, ruany@chinasic.com
//
// FileName     :IMemberInfo.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date         :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Membership
{
    #region Using Libraries
    using System;
    using System.Collections.Generic;
    #endregion

    /// <summary>��Ա</summary>
    public interface IMemberInfo : ISerializedObject
    {
        /// <summary>�ʺ�</summary>
        IAccountInfo Account { get; set; }

        /// <summary>��ʶ</summary>
        string Id { get; set; }

        /// <summary>����</summary>
        string FullName { get; set; }

        /// <summary>Ĭ�Ϲ�˾��ʶ</summary>
        string CorporationId { get; set; }

        /// <summary>Ĭ�Ϲ�˾��Ϣ</summary>
        IOrganizationInfo Corporation { get; }

        /// <summary>Ĭ��һ�����ű�ʶ</summary>
        string DepartmentId { get; set; }

        /// <summary>Ĭ��һ��������Ϣ</summary>
        IOrganizationInfo Department { get; }

        /// <summary>Ĭ�϶������ű�ʶ</summary>
        string Department2Id { get; set; }

        /// <summary>Ĭ�϶���������Ϣ</summary>
        IOrganizationInfo Department2 { get; }

        /// <summary>Ĭ���������ű�ʶ</summary>
        string Department3Id { get; set; }

        /// <summary>Ĭ������������Ϣ</summary>
        IOrganizationInfo Department3 { get; }

        /// <summary>Ĭ��������֯��ʶ</summary>
        string OrganizationId { get; set; }

        /// <summary>Ĭ��������֯��Ϣ</summary>
        IOrganizationInfo Organization { get; }

        /// <summary>Ĭ��������֯·��</summary>
        string OrganizationPath { get; }

        /// <summary>Ĭ�Ͻ�ɫ��ʶ</summary>
        string RoleId { get; set; }
        
        /// <summary>Ĭ�Ͻ�ɫ��Ϣ</summary>
        IRoleInfo Role { get; }

        /// <summary>��λͷ��</summary>
        string Headship { get; set; }

        /// <summary>�Ա�</summary>
        string Sex { get; set; }

        /// <summary>Ĭ��ְλ��ʶ</summary>
        string JobId { get; set; }

        /// <summary>Ĭ��ְλ��Ϣ</summary>
        IJobInfo Job { get; }
        
        /// <summary>Ĭ��ְ����ʶ</summary>
        string JobGradeId { get; set; }

        /// <summary>Ĭ��ְ����Ϣ</summary>
        IJobGradeInfo JobGrade { get; }

        /// <summary>Ĭ����ְ��λ��ʶ</summary>
        string AssignedJobId { get; set; }

        /// <summary>Ĭ����ְ��λ</summary>
        IAssignedJobInfo AssignedJob { get; }

        /// <summary>��ְ��Ϣ</summary>
        IList<IAssignedJobInfo> PartTimeJobs { get; }

        /// <summary>�ֻ�����</summary>
        string Mobile { get; set; }

        /// <summary>��������</summary>
        string Telephone { get; set; }

        /// <summary>�����ʺ�</summary>
        string Email { get; set; }

        /// <summary>QQ�ʺ�</summary>
        string QQ { get; set; }

        /// <summary>MSN�ʺ�</summary>
        string MSN { get; set; }

        /// <summary>Rtx�ʺ�</summary>
        string Rtx { get; set; }

        /// <summary>������֯�ܹ�ȫ·��</summary>
        string FullPath { get; set; }

        #region 属性:ExtensionInformation
        /// <summary>��Ա��չ��Ϣ</summary>
        IExtensionInformation ExtensionInformation { get; }
        #endregion
    }
}
