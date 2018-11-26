using Qmmands;
using SaltyEmu.Commands.Entities;
using System;
using System.Threading.Tasks;

namespace SaltyEmu.Commands.Checks
{
    public sealed class PlayerInMapAttribute : CheckBaseAttribute
    {
        public override Task<CheckResult> CheckAsync(ICommandContext context, IServiceProvider provider)
        {
            var ctx = context as SaltyCommandContext;

            return ctx.Player.CurrentMap is null
                ? Task.FromResult(new CheckResult("You need to be in a map to execute that command."))
                : Task.FromResult(CheckResult.Successful);
        }
    }
}
