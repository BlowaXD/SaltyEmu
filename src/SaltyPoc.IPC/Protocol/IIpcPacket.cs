using System;
using System.Threading.Tasks;

namespace SaltyPoc.IPC.Protocol
{
    public interface IIpcPacket
    {
        Guid Id { get; set; }
    }
}