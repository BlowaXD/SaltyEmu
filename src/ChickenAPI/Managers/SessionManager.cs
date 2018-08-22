using ChickenAPI.Core.Network;
using ChickenAPI.Game.Packets;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Managers
{
    public interface INostaleSession : IClient<IPacket>
    {
    }

    public class SessionManager
    {
        public INostaleSession GetSessionById(int id) => null;
    }
}