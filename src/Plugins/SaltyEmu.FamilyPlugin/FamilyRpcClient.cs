using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Data.Families;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.FamilyPlugin.Communication;

namespace SaltyEmu.FamilyPlugin
{
    public class FamilyRpcClient : MappedRpcRepository<FamilyDto>, IFamilyService
    {
        public FamilyRpcClient(IRpcClient builder) : base(builder)
        {
        }

        public FamilyDto GetByName(string name)
        {
            return Client.RequestAsync<GetFamilyInformationResponse>(new GetFamilyInformationRequest
            {
                FamilyName = name
            }).ConfigureAwait(false).GetAwaiter().GetResult().Family;
        }

        public async Task<FamilyDto> GetByNameAsync(string name)
        {
            GetFamilyInformationResponse tmp = await Client.RequestAsync<GetFamilyInformationResponse>(new GetFamilyInformationRequest
            {
                FamilyName = name
            });
            return tmp.Family;
        }

        public void UpdateFamily(long familyId)
        {
            // not yet
        }
    }
}