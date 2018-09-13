using System.Collections.Generic;
using SaltyPoc.IPC.Protocol;

namespace SaltyPoc.IPC.PacketExample
{
    internal sealed class GetFamilyMembersNameResponse : BaseResponse
    {
        public List<string> Names { get; set; }
    }
}