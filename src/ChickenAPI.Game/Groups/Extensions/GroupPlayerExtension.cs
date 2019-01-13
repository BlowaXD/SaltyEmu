using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Group;

namespace ChickenAPI.Game.Groups.Extensions
{
    public static class GroupPlayerExtension
    {
        public static PstPacket GeneratePstPacket(this IPlayerEntity entity, ref int iconIndex)
        {
            if (entity == null)
            {
                return null;
            }

            return new PstPacket
            {
                VisualType = VisualType.Character,
                GroupIndex = iconIndex++,
                VisualId = entity.Id,
                HpPercentage = entity.HpPercentage,
                HpMax = entity.HpMax,
                MpPercentage = entity.MpPercentage,
                MpMax = entity.MpMax,
                Gender = entity.Character.Gender,
                Class = entity.Character.Class,
                Morph = entity.MorphId,
                Buffs = new List<long>()
            };
        }

        public static PstPacket GeneratePstPacket(this IMateEntity entity, ref int iconIndex)
        {
            if (entity == null)
            {
                return null;
            }

            return new PstPacket
            {
                VisualType = VisualType.Character,
                GroupIndex = iconIndex++,
                VisualId = entity.Id,
                HpPercentage = entity.HpPercentage,
                HpMax = entity.HpMax,
                MpPercentage = entity.MpPercentage,
                MpMax = entity.MpMax,
                Gender = GenderType.Male, // 0
                Class = CharacterClassType.Adventurer, // 0
                Morph = entity.MorphId,
                Buffs = new List<long>()
            };
        }

        public static PInitPacket.PInitMateSubPacket GeneratePInitSubPacket(this IMateEntity mate, ref byte groupIndex)
        {
            if (mate == null)
            {
                return null;
            }

            return new PInitPacket.PInitMateSubPacket
            {
                VisualType = mate.Type,
                VisualId = mate.Id,
                GroupIndex = groupIndex++,
                Name = mate.Mate.Name.Replace(' ', '^'),
                Level = mate.Level,
                MorphOrNpcMonsterId = mate.MorphId == 0 ? mate.NpcMonster.Id : mate.MorphId,
                Unknown = -1,
                Unknown2 = 0
            };
        }

        public static PInitPacket.PInitPlayerSubPacket GeneratePinitSubPacket(this IPlayerEntity player, ref byte groupIndex)
        {
            if (player == null)
            {
                return null;
            }

            return new PInitPacket.PInitPlayerSubPacket
            {
                VisualType = player.Type,
                VisualId = player.Id,
                MorphId = player.MorphId,
                Name = player.Character.Name,
                Gender = player.Character.Gender,
                GroupIndex = groupIndex++,
                Level = player.Level,
                Class = player.Character.Class,
                HeroLevel = player.HeroLevel
            };
        }

        public static PInitPacket GeneratePInitPacket(this IPlayerEntity player)
        {
            byte tmp = 0;

            List<PInitPacket.PInitMateSubPacket> mates = new List<PInitPacket.PInitMateSubPacket>();
            List<PInitPacket.PInitPlayerSubPacket> players = new List<PInitPacket.PInitPlayerSubPacket>();
            mates.AddRange(player.ActualMates.Where(s => s != null).Select(s => s?.GeneratePInitSubPacket(ref tmp)).Where(s => s != null));
            players.AddRange(player.Group.Players.Where(s => s != null && s != player).Select(s => s.GeneratePinitSubPacket(ref tmp)).Where(s => s != null));

            return new PInitPacket
            {
                PartySize = player.Mates.Count(s => s.Mate.IsTeamMember) + player.GroupMembersCount - 1,
                MateSubPackets = mates,
                PlayerSubPackets = players
            };
        }

        public static IEnumerable<PstPacket> GeneratePstPackets(this IPlayerEntity player)
        {
            List<PstPacket> tmp = new List<PstPacket>();
            int i = 0;
            tmp.AddRange(player.ActualMates.Select(pet => pet.GeneratePstPacket(ref i)));

            GroupDto group = player.Group;
            if (group == null)
            {
                return null;
            }

            foreach (IPlayerEntity member in group.Players)
            {
                tmp.Add(player.GeneratePstPacket(ref i));
            }

            return tmp;
        }
    }
}