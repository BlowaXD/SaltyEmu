using System.Threading.Tasks;
using Qmmands;

namespace SaltyEmu.Commands
{
    public class SaltyModuleBase : ModuleBase<SaltyCommandContext>
    {
        /// <summary>
        ///     This intends to fill the current Context with the command being executed.
        /// </summary>
        protected override Task BeforeExecutedAsync(Command command)
        {
            Context.Command = command;

            return base.BeforeExecutedAsync(command);
        }
    }
}
