using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Movements;
using ChickenAPI.Packets.Game.Server.Entities;

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

            return entity.GenerateTpPacket(movable.Movable.Actual.X, movable.Movable.Actual.Y);
        }

        public static TpPacket GenerateTpPacket(this IEntity entity, short x, short y)
        {
            var packet = new TpPacket { X = x, Y = y };
            switch (entity)
            {
                case IPlayerEntity player:
                    packet.VisualId = player.Character.Id;
                    packet.VisualType = VisualType.Character;
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