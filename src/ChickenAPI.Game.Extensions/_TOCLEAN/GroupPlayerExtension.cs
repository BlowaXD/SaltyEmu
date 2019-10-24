using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Groups;
using ChickenAPI.Packets.ServerPackets.Shop;

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
                Type = VisualType.Player,
                GroupOrder = ++iconIndex,
                VisualId = entity.Id,
                HpLeft = entity.HpPercentage,
                HpLoad = entity.HpMax,
                MpLeft = entity.MpPercentage,
                MpLoad = entity.MpMax,
                Gender = entity.Character.Gender, // 0
                Race = (short)entity.Character.Class, // 0
                Morph = entity.MorphId,
                BuffIds = new List<int>(entity.Buffs.Select(s => (int)s.Id))
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
                Type = VisualType.Player,
                GroupOrder = iconIndex++,
                VisualId = entity.Id,
                HpLeft = entity.HpPercentage,
                HpLoad = entity.HpMax,
                MpLeft = entity.MpPercentage,
                MpLoad = entity.MpMax,
                Gender = GenderType.Male, // 0
                Race = (short)CharacterClassType.Adventurer, // 0
                Morph = entity.MorphId,
                BuffIds = new List<int>(entity.Buffs.Select(s => (int)s.Id))
            };
        }

        public static PinitSubPacket GeneratePInitSubPacket(this IMateEntity mate, ref byte groupIndex)
        {
            if (mate == null)
            {
                return null;
            }

            return new PinitSubPacket
            {
                VisualType = mate.Type,
                VisualId = mate.Id,
                GroupPosition = ++groupIndex,
                Name = mate.Mate.Name.Replace(' ', '^'),
                Level = mate.Level,
                Morph = (short)(mate.MorphId == 0 ? mate.NpcMonster.Id : mate.MorphId),
                Unknown = -1,
                Gender = 0,
                Race = 0,
                HeroLevel = 0
            };
        }

        public static PinitSubPacket GeneratePinitSubPacket(this IPlayerEntity player, ref byte groupIndex)
        {
            if (player == null)
            {
                return null;
            }

            return new PinitSubPacket()
            {
                VisualType = player.Type,
                VisualId = player.Id,
                Morph = player.MorphId,
                GroupPosition = ++groupIndex,
                Name = player.Character.Name,
                Unknown = 0,
                Gender = player.Character.Gender,
                Level = player.Level,
                Race = (short)player.Character.Class,
                HeroLevel = player.HeroLevel
            };
        }

        public static PidxPacket GeneratePidxPacket(this IPlayerEntity player)
        {
            List<PidxSubPacket> packets = new List<PidxSubPacket> { new PidxSubPacket { VisualId = player.Id, IsGrouped = player.HasGroup } };

            if (!player.HasGroup)
            {
                return new PidxPacket
                {
                    GroupId = player.HasGroup ? player.Group.Id : -1,
                    SubPackets = packets
                };
            }

            foreach (IPlayerEntity member in player.Group.Players)
            {
                if (member == player)
                {
                    continue;
                }

                packets.Add(new PidxSubPacket
                {
                    VisualId = member.Id,
                    IsGrouped = member.Group == player.Group // stupid as fuck ??
                });
            }

            return new PidxPacket
            {
                GroupId = player.HasGroup ? player.Group.Id : -1,
                SubPackets = packets
            };
        }

        public static PinitPacket GeneratePInitPacket(this IPlayerEntity player)
        {
            byte tmp = 0;

            List<PinitSubPacket> subPackets = new List<PinitSubPacket>();
            subPackets.AddRange(player.ActualMates.Where(s => s != null)?.Select(s => s.GeneratePInitSubPacket(ref tmp)).Where(s => s != null));

            if (player.HasGroup)
            {
                subPackets.AddRange(player.Group.Players.Where(s => s != null).Select(s => s.GeneratePinitSubPacket(ref tmp)).Where(s => s != null));
            }

            return new PinitPacket
            {
                GroupSize = player.Mates.Count(s => s.Mate.IsTeamMember) + player.GroupMembersCount,
                PinitSubPackets = subPackets
            };
        }

        public static IEnumerable<PstPacket> GeneratePstPackets(this IPlayerEntity player)
        {
            List<PstPacket> tmp = new List<PstPacket>();
            int i = 0;
            tmp.AddRange(player.ActualMates.Select(pet => pet.GeneratePstPacket(ref i)));

            Group group = player.Group;
            if (group == null)
            {
                return tmp;
            }

            foreach (IPlayerEntity member in group.Players)
            {
                tmp.Add(member.GeneratePstPacket(ref i));
            }

            return tmp;
        }
    }
}