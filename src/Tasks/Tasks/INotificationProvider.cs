using X3Platform.Tasks.Model;
namespace X3Platform.Tasks
{
    /// <summary>通知接口</summary>
    public interface INotificationProvider
    {
        #region 函数:Send(TaskWorkInfo task, string receiverIds, string options)
        /// <summary>发送通知</summary>
        /// <param name="task">任务信息</param>
        /// <param name="receiverIds">接收者</param>
        /// <param name="options">选项</param>
        int Send(TaskWorkInfo task, string receiverIds, string options);
        #endregion
    }
}
