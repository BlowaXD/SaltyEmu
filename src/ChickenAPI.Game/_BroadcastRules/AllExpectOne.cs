using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Network;

namespace ChickenAPI.Game._BroadcastRules
{
    public class AllExpectOne : IBroadcastRule
    {
        private readonly IPlayerEntity _sender;

        public AllExpectOne(IPlayerEntity sender) => _sender = sender;

        public bool Match(IPlayerEntity player) => player != _sender;
    }
}