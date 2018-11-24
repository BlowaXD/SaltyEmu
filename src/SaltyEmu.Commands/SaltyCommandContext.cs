using ChickenAPI.Game.Entities.Player;
using Qmmands;

namespace SaltyEmu.Commands
{
    internal class SaltyCommandContext : ICommandContext
    {
        public Command Command { get; set; }
        public string Message { get; set; }
        public object Sender { get; set; }

        public SaltyCommandContext(string message, object sender)
            => Message = message;
    }
}