using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Relations;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Relations;

namespace ChickenAPI.Game.Relations.Extensions
{
    public static class PacketExtensions
    {
        public static FInitPacket GenerateFInitPacket(this IPlayerEntity player)
        {
            List<FInitPacket.FInitSubPacket> subpackets = new List<FInitPacket.FInitSubPacket>();

            subpackets.AddRange(
                player.Relations.Relation.Where(s => s != null && (s.Type == CharacterRelationType.Friend || s.Type == CharacterRelationType.Spouse))?.Select(relation =>
                    new FInitPacket.FInitSubPacket
                    {
                        RelationId = relation.TargetId,
                        RelationType = relation.Type,
                        CharacterName = relation.Name,
                        IsOnline = true
                    }));
            return new FInitPacket
            {
                Packets = subpackets
            };
        }

        public static BlInitPacket GenerateBlIinitPacket(this IPlayerEntity player)
        {
            List<BlInitPacket.BlInitSubPacket> subPackets = new List<BlInitPacket.BlInitSubPacket>();


            subPackets.AddRange(player.Relations.Relation.Where(s => s?.Type == CharacterRelationType.Blocked).Select(relation => new BlInitPacket.BlInitSubPacket
            {
                CharacterId = relation.TargetId,
                CharacterName = relation.Name
            }));
            return new BlInitPacket
            {
                SubPackets = subPackets
            };
        }
    }
}