using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Args;

namespace ChickenAPI.Game.GuriHandling.Handling
{
    public interface IGuriHandler
    {
        void Register(GuriRequestHandler handler);
        void Unregister(long guriEffectId);

        void Handle(IPlayerEntity player, GuriEvent args);
    }
}