using ChickenAPI.Data.Families;
using SaltyEmu.Communication.Protocol;

namespace SaltyEmu.FamilyPlugin.Communication
{
    internal sealed class GetFamilyInformation : BaseRequest
    {
        public long FamilyId { get; set; }

        public string FamilyName { get; set; }
    }

    internal sealed class GetFamilyInformationResponse : BaseResponse
    {
        public FamilyDto Family { get; set; }
    }
}