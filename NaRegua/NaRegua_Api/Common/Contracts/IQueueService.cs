namespace NaRegua_Api.Common.Contracts
{
    public interface IQueueService
    {
        void PublishMessage(string message);
    }
}
