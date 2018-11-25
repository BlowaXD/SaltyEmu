using ChickenAPI.Game.Entities.Player;
using Qmmands;

namespace SaltyEmu.Commands.Entities
{
    public class SaltyCommandContext : ICommandContext
    {
        public Command Command { get; set; }

        public string Message { get; set; }
        public IPlayerEntity Sender { get; set; }

        public string Input { get; set; }

        public SaltyCommandContext(string message, IPlayerEntity sender)
        {
            Message = message;
            Sender = sender;

            var pos = message.IndexOf('$') + 2;
            Input = message.Substring(pos);
        }
    }
}