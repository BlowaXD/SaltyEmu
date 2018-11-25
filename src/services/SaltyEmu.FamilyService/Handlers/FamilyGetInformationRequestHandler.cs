using System.Threading.Tasks;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FamilyPlugin.Communication;

namespace SaltyEmu.FamilyService.Handlers
{
    public class FamilyGetInformationRequestHandler : GenericIpcRequestHandler<GetFamilyInformationRequest>
    {
        public override async Task Handle(GetFamilyInformationRequest request)
        {
            await request.ReplyAsync(new GetFamilyInformationResponse
            {
                Family = null
            });
        }
    }
}