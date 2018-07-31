using ChickenAPI.Core.Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Managers
{
    public interface INostaleSession : IClient<IPacket>
    {

    }
    public class SessionManager
    {
        public INostaleSession GetSessionById(int id)
        {
            return null;
        }
    }
}