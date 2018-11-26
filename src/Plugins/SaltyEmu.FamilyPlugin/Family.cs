using System.Threading.Tasks;
using ChickenAPI.Data.Families;
using ChickenAPI.Game.Data.AccessLayer.Families;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.FamilyPlugin.Communication;

namespace SaltyEmu.FamilyPlugin
{
    public class Family : MappedRepositoryMqtt<FamilyDto>, IFamilyService
    {
        public Family(MqttClientConfigurationBuilder builder) : base(builder)
        {
        }

        public FamilyDto GetByName(string name)
        {
            return RequestAsync<GetFamilyInformationResponse>(new GetFamilyInformationRequest
            {
                FamilyName = name
            }).ConfigureAwait(false).GetAwaiter().GetResult().Family;
        }

        public async Task<FamilyDto> GetByNameAsync(string name)
        {
            GetFamilyInformationResponse tmp = await RequestAsync<GetFamilyInformationResponse>(new GetFamilyInformationRequest
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