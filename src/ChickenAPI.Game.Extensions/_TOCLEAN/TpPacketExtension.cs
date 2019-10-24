using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Entities;

namespace ChickenAPI.Game.Entities.Extensions
{
    public static class TpPacketExtension
    {
        public static TpPacket GenerateTpPacket(this IEntity entity)
        {
            if (!(entity is IMovableEntity movable))
            {
                return null;
            }

            return entity.GenerateTpPacket(movable.Position.X, movable.Position.Y);
        }

        public static TpPacket GenerateTpPacket(this IEntity entity, short x, short y)
        {
            var packet = new TpPacket { X = x, Y = y };
            switch (entity)
            {
                case IPlayerEntity player:
                    packet.VisualId = player.Character.Id;
                    packet.VisualType = VisualType.Player;
                    return packet;
                case IMonsterEntity monster:
                    packet.VisualType = VisualType.Monster;
                    packet.VisualId = monster.MapMonster.Id;
                    return packet;
                case INpcEntity npc:
                    packet.VisualType = VisualType.Npc;
                    packet.VisualId = npc.MapNpc.Id;
                    return packet;
            }

            return null;
        }
    }
}