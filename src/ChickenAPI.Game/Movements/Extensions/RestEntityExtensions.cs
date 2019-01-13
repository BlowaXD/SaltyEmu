﻿using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Entities;

namespace ChickenAPI.Game.Movements.Extensions
{
    public static class RestEntityExtensions
    {
        public static RestPacket GenerateRestPacket(this IEntity entity)
        {
            switch (entity)
            {
                case IMonsterEntity monster:
                    return new RestPacket
                    {
                        VisualType = VisualType.Monster,
                        VisualId = monster.MapMonster.Id,
                        IsSitting = monster.IsSitting
                    };

                case INpcEntity npc:
                    return new RestPacket
                    {
                        VisualType = VisualType.Npc,
                        VisualId = npc.MapNpc.Id,
                        IsSitting = npc.IsSitting
                    };
                case IPlayerEntity player:
                    return new RestPacket
                    {
                        VisualType = VisualType.Character,
                        VisualId = player.Character.Id,
                        IsSitting = player.IsSitting
                    };
                default:
                    return null;
            }
        }
    }
}