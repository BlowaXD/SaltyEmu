using Qmmands;

namespace SaltyEmu.Commands.Entities
{
    public sealed class SaltyCommandResult : CommandResult
    {
        public override bool IsSuccessful { get; }
        public string Message { get; }

        public SaltyCommandResult(bool isSuccessful, string message = null)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}