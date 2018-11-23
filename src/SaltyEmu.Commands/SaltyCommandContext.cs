using Qmmands;

namespace SaltyEmu.Commands
{
    internal class SaltyCommandContext : ICommandContext
    {
        public Command Command { get; set; }
        public string Message { get; set; }
        public object Entity { get; set; }

        public SaltyCommandContext(string message, object entity)
            => Message = message;
    }
}