namespace ChickenAPI.Packets
{
    public interface IPacket
    {
        string Header { get; }
        string Content { get; }
    }
}