using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Families;

namespace ChickenAPI.Game.Families.Extensions
{
    public static class GInfoPacketExtensions
    {
        public static GInfoPacket GenerateGInfoPacket(this IPlayerEntity player)
        {
            if (!player.HasFamily)
            {
                return null;
            }

            return new GInfoPacket
            {
                FamilyName = player.Family.Name,
                CharacterName = player.Character.Name,
                FamilyLevel = player.Family.FamilyLevel,
                FamilyManagerAuthorityType = player.Family.ManagerAuthorityType,
                FamilyMemberAuthorityType = player.Family.MemberAuthorityType,
                CharacterFamilyAuthority = player.FamilyCharacter.Authority,
                FamilyHeadGenderType = player.Character.Gender,
                FamilyManagerCanGetHistory = player.Family.ManagerCanGetHistory,
                FamilyManagerCanInvit = player.Family.ManagerCanInvite,
                FamilyManagerCanShout = player.Family.ManagerCanShout,
                FamilyManagerCanNotice = player.Family.ManagerCanNotice,
                FamilyMemberCanGetHistory = player.Family.MemberCanGetHistory,
                FamilyMessage = "",
                FamilyXp = player.Family.FamilyExperience,
                MaxFamilyXp = 100000, // todo algorithm
                MembersCapacity = 50, // todo level capacity
                MembersCount = 1 // todo 
            };
        }
    }
}