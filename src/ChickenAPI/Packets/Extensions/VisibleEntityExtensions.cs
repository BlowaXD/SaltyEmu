using System;
using ChickenAPI.Data.TransferObjects.NpcMonster;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Packets.Game.Server;

namespace ChickenAPI.Game.Packets.Extensions
{
    public static class VisibleEntityExtensions
    {
        private static InPacketBase GenerateInMonster(IMonsterEntity monster)
        {
            BattleComponent battle = monster.Battle;
            NpcMonsterDto npcMonster = monster.NpcMonster;
            MovableComponent movable = monster.Movable;
            return new InPacketBase
            {
                VisualType = VisualType.Monster,
                Name = npcMonster.Id.ToString(),
                Unknown = monster.MapMonster.ToString(),
                PositionX = movable.Actual.X,
                PositionY = movable.Actual.Y,
                DirectionType = movable.DirectionType,
                InMonsterSubPacket = new InMonsterSubPacket
                {
                    HpPercentage = Convert.ToByte(Math.Ceiling(battle.Hp / (battle.HpMax * 100.0))),
                    MpPercentage = Convert.ToByte(Math.Ceiling(battle.Mp / (battle.MpMax * 100.0))),
                    Unknown1 = 0,
                    Unknown2 = 0,
                    Unknown3 = -1,
                    NoAggressiveIcon = npcMonster.NoAggresiveIcon,
                    Unknown4 = 0,
                    Unknown5 = -1,
                    Unknown6 = "-",
                    Unknown7 = 0,
                    Unknown8 = -1,
                    Unknown9 = 0,
                    Unknown10 = 0,
                    Unknown11 = 0,
                    Unknown12 = 0,
                    Unknown13 = 0,
                    Unknown14 = 0,
                    Unknown15 = 0,
                    Unknown16 = 0
                },
            };
        }

        public static InPacketBase GenerateIn(IEntity entity)
        {
            switch (entity.Type)
            {
                case EntityType.Monster:
                    return GenerateInMonster(entity as IMonsterEntity);
                default:
                    return new InPacketBase();
            }
        }
    }
}