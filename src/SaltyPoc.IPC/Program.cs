using System.Threading.Tasks;

namespace SaltyPoc.IPC
{
    class Program
    {
        static void Main(string[] args)
        {
            DoTheWork().Wait();
        }

        private static async Task DoTheWork()
        {
        }
    }
}