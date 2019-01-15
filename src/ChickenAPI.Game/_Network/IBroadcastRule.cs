using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game._Network
{
    public interface IBroadcastRule
    {
        bool Match(IPlayerEntity player);
    }
}