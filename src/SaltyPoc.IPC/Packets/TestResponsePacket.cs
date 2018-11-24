using System;
using ChickenAPI.Core.IPC.Protocol;
using SaltyEmu.Communication.Protocol;

namespace SaltyPoc.IPC.Packets
{
    public class TestResponsePacket : BaseResponse
    {
        public string Name => "ok";
        public string RandomPropertyAsATry => "another_property 123";
    }
}