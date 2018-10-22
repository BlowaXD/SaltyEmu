using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Network.BroadcastRules
{
    public class AllExpectOne : IBroadcastRule
    {
        private readonly IPlayerEntity _sender;

        public AllExpectOne(IPlayerEntity sender) => _sender = sender;

        public bool Match(IPlayerEntity player) => player != _sender;
    }
}