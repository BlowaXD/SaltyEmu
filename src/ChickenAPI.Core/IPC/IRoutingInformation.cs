namespace ChickenAPI.Core.IPC
{
    public interface IRoutingInformation
    {
        string IncomingTopic { get; }
        string OutgoingTopic { get; }
    }
}