using System;
using ChickenAPI.Core.IPC.Protocol;
using SaltyEmu.IpcPlugin.Protocol;

namespace SaltyPoc.IPC.Packets
{
    public class TestResponsePacket : BaseResponse
    {
        public string Name => "ok";
        public string Popopopo => "another_property 123";
    }
}