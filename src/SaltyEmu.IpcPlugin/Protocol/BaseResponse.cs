using System;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.IpcPlugin.Protocol
{
    public class BaseResponse : IIpcResponse
    {
        public Guid Id { get; set; }
        public Guid RequestId { get; set; }
    }
}