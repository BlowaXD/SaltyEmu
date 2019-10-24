using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Relations;

namespace ChickenAPI.Game.Relations.Extensions
{
    public static class PacketExtensions
    {
        public static FinitPacket GenerateFInitPacket(this IPlayerEntity player)
        {
            List<FinitSubPacket> subpackets = new List<FinitSubPacket>();

            subpackets.AddRange(
                player.Relations.Relation.Where(s => s != null && (s.Type == CharacterRelationType.Friend || s.Type == CharacterRelationType.Spouse))?.Select(relation =>
                    new FinitSubPacket
                    {
                        CharacterId = relation.TargetId,
                        CharacterName = relation.Name,
                        RelationType = relation.Type,
                        IsOnline = true
                    }));
            return new FinitPacket()
            {
                SubPackets = subpackets
            };
        }

        public static BlinitPacket GenerateBlIinitPacket(this IPlayerEntity player)
        {
            List<BlinitSubPacket> subPackets = new List<BlinitSubPacket>();


            subPackets.AddRange(player.Relations.Relation.Where(s => s?.Type == CharacterRelationType.Blocked).Select(relation => new BlinitSubPacket
            {
                RelatedCharacterId = relation.TargetId,
                CharacterName = relation.Name
            }));
            return new BlinitPacket()
            {
                SubPackets = subPackets
            };
        }
    }
}