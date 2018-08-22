namespace ChickenAPI.Core.ISC
{
    public interface IISCCommunicator
    {
        void BroadcastPacket<T>(T packet) where T : IISCPacket;
        T Request<T>(IISCRequest request) where T : IISCResponse;
    }
}