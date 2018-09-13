using SaltyPoc.IPC.Protocol;

namespace SaltyPoc.IPC.PacketExample
{
    internal sealed class GetFamilyMembersName : BaseRequest
    {
        public long FamilyId { get; set; }
    }
}