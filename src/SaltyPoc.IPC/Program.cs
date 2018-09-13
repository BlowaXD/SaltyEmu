using System.Threading.Tasks;
using SaltyPoc.IPC.PacketExample;
using SaltyPoc.IPC.Protocol;

namespace SaltyPoc.IPC
{
    class Program
    {
        private static async void OnRequestReceived(IIpcRequest packet)
        {
            await packet.RespondAsync(new GetFamilyMembersNameResponse());
        }

        static void Main(string[] args)
        {
            DoTheWork().Wait();
        }

        private static Task DoTheWork()
        {
            return Task.CompletedTask;
        }
    }
}