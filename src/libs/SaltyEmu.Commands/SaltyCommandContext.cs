using ChickenAPI.Game.Entities.Player;
using Qmmands;

namespace SaltyEmu.Commands
{
    public class SaltyCommandContext : ICommandContext
    {
        public Command Command { get; set; }

        public string Message { get; set; }
        public IPlayerEntity Sender { get; set; }

        public SaltyCommandContext(string message, IPlayerEntity sender)
        {
            Message = message;
            Sender = sender;
        }
    }
}