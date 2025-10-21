namespace Assets.C_.bus
{
    /// <summary>
    /// �����¼��������ӿ�
    /// </summary>
    /// <typeparam name="TEvent">�¼�����</typeparam>
    public interface IEventHandler<in TEvent> where TEvent : class
    {
        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="eventData">�¼�����</param>
        void Handle(TEvent eventData);
    }
}