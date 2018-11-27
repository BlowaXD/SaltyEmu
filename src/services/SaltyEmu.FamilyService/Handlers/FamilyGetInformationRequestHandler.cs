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

            if (request.FamilyName == "test")
            {
                await request.ReplyAsync(new GetFamilyInformationResponse
                {
                    Family = null
                });
            }
            else
            {
                await request.ReplyAsync(new GetFamilyInformationResponse
                {
                    Family = new FamilyDto
                    {
                        Id = 1,
                        Name = "real",
                        FamilyLevel = 10,
                    }
                });
            }
        }
    }
}