using ChickenAPI.Enums;
using ChickenAPI.Game.Features.Families;
using ChickenAPI.Game.Features.Specialists;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Entities.Player
{
    public static class CInfoPacketExtension
    {
        public static CInfoPacket GenerateCInfoPacket(this IPlayerEntity player)
        {
            var family = player.GetComponent<FamilyComponent>();
            var sp = player.GetComponent<SpecialistComponent>();
            return new CInfoPacket
            {
                Name = player.Character.Name,
                Unknown1 = "-", //TODO: Find signification
                GroupId = -1,
                FamilyId = -1, // todo : family system
                FamilyName = family?.FamilyName,
                CharacterId = player.Character.Id,
                Authority = player.Session.Account.Authority > AuthorityType.GameMaster ? (byte)2 : (byte)0,
                Gender = player.Character.Gender,
                HairStyle = player.Character.HairStyle,
                HairColor = player.Character.HairColor,
                Class = player.Character.Class,
                Icon = 0, // todo
                Compliment = player.Character.Compliment,
                Morph = 0,
                Invisible = !player.Visibility.IsVisible,
                FamilyLevel = 0,
                SpUpgrade = sp.Upgrade,
                ArenaWinner = player.Character.ArenaWinner,
            };
        }
    }
}