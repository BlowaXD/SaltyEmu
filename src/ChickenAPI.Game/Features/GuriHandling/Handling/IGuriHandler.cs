using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.GuriHandling.Args;

namespace ChickenAPI.Game.Features.GuriHandling.Handling
{
    public interface IGuriHandler
    {
        void Register(GuriRequestHandler handler);
        void Unregister(long guriEffectId);

        void Handle(IPlayerEntity player, GuriEventArgs args);
    }
}