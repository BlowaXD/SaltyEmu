using SaltyEmu.Communication.Protocol;

namespace SaltyPoc.IPC.Packets
{
    public class TestRequestPacket : BaseRequest
    {
        public string Name => "Test_Request_Packet";
    }
}