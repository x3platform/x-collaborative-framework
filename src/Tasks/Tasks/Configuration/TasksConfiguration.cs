#region Copyright & Author
// =============================================================================
//
// Copyright (c) ruanyu@live.com
//
// FileName     :TasksConfiguration.cs
//
// Description  :
//
// Author       :ruanyu@x3platfrom.com
//
// Date		    :2010-01-01
//
// =============================================================================
#endregion

namespace X3Platform.Tasks.Configuration
{
    #region Using Libraries
    using System.Configuration;

    using X3Platform.Configuration;
    #endregion

    /// <summary>����������Ϣ</summary>
    public class TasksConfiguration : X3Platform.Configuration.XmlConfiguraton
    {
        /// <summary>����Ӧ�õ�����</summary>
        public const string ApplicationName = "Tasks";

        /// <summary>������������</summary>
        public const string SectionName = "tasksConfiguration";
        
        /// <summary>��ȡ������������</summary>
        public override string GetSectionName()
        {
            return SectionName;
        }
    }
}
