using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Services
{
    /// <summary>����������ӿ�</summary>
    public interface IServiceObserver
    {
        /// <summary>����</summary>
        string Name { get; }

        /// <summary>�������������˯��״̬</summary>
        bool Sleeping { get; }

        /// <summary>����������Ƿ�������</summary>
        bool IsRunning { get; }

        /// <summary>���з��������</summary>
        void Start();
        
        /// <summary>���з��������</summary>
        void Run();
        
        /// <summary>�رշ��������</summary>
        void Close();
    }
}
