using ChickenAPI.Data.Families;
using SaltyEmu.Communication.Protocol;

namespace SaltyEmu.FamilyPlugin.Communication
{
    public class GetFamilyInformationRequest : BaseRequest
    {
        public long FamilyId { get; set; }

        public string FamilyName { get; set; }
    }

    public class GetFamilyInformationResponse : BaseResponse
    {
        public FamilyDto Family { get; set; }
    }
}