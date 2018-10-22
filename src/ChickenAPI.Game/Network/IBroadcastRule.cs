using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Network
{
    public interface IBroadcastRule
    {
        bool Match(IPlayerEntity player);
    }
}