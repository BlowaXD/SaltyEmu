namespace SaltyPoc.IPC
{
    internal sealed class ExampleRequest : BaseRequest, IIpcPacket
    {
        public long FamilyId { get; set; }
    }
}