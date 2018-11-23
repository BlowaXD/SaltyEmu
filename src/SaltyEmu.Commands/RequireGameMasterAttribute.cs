using System;
using System.Threading.Tasks;
using Qmmands;

namespace SaltyEmu.Commands
{
    public sealed class RequireGameMasterAttribute : CheckBaseAttribute
    {
        /// <summary>
        /// This is a check (pre-condition) before trying to execute a command that needs to pass this check.
        /// </summary>
        /// <param name="context">Context of the command. It needs to be castable to a SaltyCommandContext in our case.</param>
        /// <param name="provider">ServiceProvider for dependency injection.</param>
        /// <returns></returns>
        public override Task<CheckResult> CheckAsync(ICommandContext context, IServiceProvider _)
        {
            var ctx = context as SaltyCommandContext;

            if (ctx is null)
            {
                return Task.FromResult(new CheckResult("Invalid context."));
            }

            return Task.FromResult(CheckResult.Successful);
        }
    }
}
