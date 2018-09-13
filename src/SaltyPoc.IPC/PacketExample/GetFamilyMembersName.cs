using SaltyPoc.IPC.Protocol;

namespace SaltyPoc.IPC.PacketExample
{
    public sealed class GetFamilyMembersName : BaseRequest
    {
        public long FamilyId { get; set; }
    }
}