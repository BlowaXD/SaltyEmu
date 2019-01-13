using System.Threading.Tasks;
using ChickenAPI.Core.IPC.Protocol;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Families;
using SaltyEmu.Communication.Protocol.RepositoryPacket;
using SaltyEmu.FamilyPlugin.Communication;

namespace SaltyEmu.FamilyService.Handlers
{
    public class FamilyGetInformationRequestHandler
    {
        private static readonly Logger Log = Logger.GetLogger<FamilyGetInformationRequestHandler>();

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
                        Name = request.FamilyName,
                        FamilyLevel = 10,
                    }
                });
            }
        }

        public static async Task OnSaveMessage(IIpcRequest packet)
        {
            if (!(packet is RepositorySaveRequest<FamilyDto> request))
            {
                Log.Warn($"Request wrong type : {packet.GetType()}");
                await packet.ReplyAsync(new RepositoryGetResponse<FamilyDto>
                {
                    Objects = null
                });
                return;
            }

            await request.ReplyAsync(new RepositorySaveRequest<FamilyDto>.Response
            {
                Objects = request.Objects
            });
        }
    }
}