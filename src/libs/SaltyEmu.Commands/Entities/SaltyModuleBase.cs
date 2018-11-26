using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using Qmmands;

namespace SaltyEmu.Commands.Entities
{
    public class SaltyModuleBase : ModuleBase<SaltyCommandContext>
    {
        protected IPlayerEntity Player { get; private set; }

        /// <summary>
        ///     This intends to fill the current Context with the command being executed.
        /// </summary>
        protected override Task BeforeExecutedAsync(Command command)
        {
            Player = Context.Sender;
            Context.Command = command;

            return base.BeforeExecutedAsync(command);
        }
    }
}