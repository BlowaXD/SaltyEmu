using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Utils
{
    public abstract class GenericIpcRequestHandler<T> where T : IIpcRequest
    {
        public abstract Task Handle(T request);
    }
}