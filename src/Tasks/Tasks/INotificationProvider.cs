using X3Platform.Tasks.Model;
namespace X3Platform.Tasks
{
    /// <summary>֪ͨ�ӿ�</summary>
    public interface INotificationProvider
    {
        #region ����:Send(TaskWorkInfo task, string receiverIds, string options)
        /// <summary>����֪ͨ</summary>
        /// <param name="task">������Ϣ</param>
        /// <param name="receiverIds">������</param>
        /// <param name="options">ѡ��</param>
        int Send(TaskWorkInfo task, string receiverIds, string options);
        #endregion
    }
}
