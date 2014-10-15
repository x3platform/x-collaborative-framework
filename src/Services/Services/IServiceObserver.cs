using System;
using System.Collections.Generic;
using System.Text;

namespace X3Platform.Services
{
    /// <summary>服务监听器接口</summary>
    public interface IServiceObserver
    {
        /// <summary>名称</summary>
        string Name { get; }

        /// <summary>服务监听器正在睡眠状态</summary>
        bool Sleeping { get; }

        /// <summary>服务监听器是否在运行</summary>
        bool IsRunning { get; }

        /// <summary>运行服务监听器</summary>
        void Start();
        
        /// <summary>运行服务监听器</summary>
        void Run();
        
        /// <summary>关闭服务监听器</summary>
        void Close();
    }
}
