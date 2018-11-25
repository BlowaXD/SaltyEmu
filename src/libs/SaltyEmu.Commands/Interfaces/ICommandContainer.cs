using System.Threading.Tasks;

namespace SaltyEmu.Commands.Interfaces
{
    public interface ICommandContainer
    {
        Task InitializeAsync();
        Task HandleMessageAsync(string message, object entity);
    }
}
