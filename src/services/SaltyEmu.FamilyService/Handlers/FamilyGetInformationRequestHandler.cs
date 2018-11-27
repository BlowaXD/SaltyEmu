using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Data.Families;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FamilyPlugin.Communication;

namespace SaltyEmu.FamilyService.Handlers
{
    public class FamilyGetInformationRequestHandler
    {
        public static async Task OnMessage(IIpcRequest packet)
        {
            if (!(packet is GetFamilyInformationRequest request))
            {
                return;
            }

            await request.ReplyAsync(new GetFamilyInformationResponse
            {
                Family = new FamilyDto
                {
                    Id = 1,
                    Name = "Test_RPC",
                    FamilyLevel = 10,
                }
            });
        }
    }
}