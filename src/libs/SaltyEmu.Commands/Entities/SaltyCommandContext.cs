using ChickenAPI.Game.Entities.Player;
using Qmmands;

namespace SaltyEmu.Commands.Entities
{
    public sealed class SaltyCommandContext : ICommandContext
    {
        public CommandService CommandService { get; }

        public Command Command { get; set; }

        public string Message { get; set; }
        public IPlayerEntity Player { get; set; }

        public string Input { get; set; }

        public SaltyCommandContext(string message, IPlayerEntity sender, CommandService cmds)
        {
            CommandService = cmds;

            Message = message;
            Player = sender;

            var pos = message.IndexOf('$') + 2;
            Input = message.Substring(pos);
        }
    }
}