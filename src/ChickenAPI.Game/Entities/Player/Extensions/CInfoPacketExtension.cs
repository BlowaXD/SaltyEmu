using ChickenAPI.Data.Families;
using ChickenAPI.Enums;
using ChickenAPI.Game.Features.Families;
using ChickenAPI.Game.Features.Specialists;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class CInfoPacketExtension
    {
        public static CInfoPacket GenerateCInfoPacket(this IPlayerEntity player)
        {
            FamilyDto family = player.Family;
            var sp = player.GetComponent<SpecialistComponent>();
            return new CInfoPacket
            {
                Name = player.Character.Name,
                Unknown1 = "-", //TODO: Find signification
                GroupId = -1,
                FamilyId = family?.Id ?? -1, // todo : family system
                FamilyName = family?.Name ?? "-",
                CharacterId = player.Character.Id,
                Authority = player.Session.Account.Authority > AuthorityType.GameMaster ? (byte)2 : (byte)0,
                Gender = player.Character.Gender,
                HairStyle = player.Character.HairStyle,
                HairColor = player.Character.HairColor,
                Class = player.Character.Class,
                Icon = (byte)player.GetReputIcon(), // todo
                Compliment = player.Character.Compliment,
                Morph = 0,
                Invisible = player.IsInvisible,
                FamilyLevel = 0,
                SpUpgrade = sp?.Upgrade ?? 0,
                ArenaWinner = player.Character.ArenaWinner
            };
        }
    }
}