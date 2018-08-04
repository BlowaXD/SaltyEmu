using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Map;

namespace ChickenAPI.Game.Entities.Monster
{
    public class NpcMonsterComponent : IComponent
    {
        public NpcMonsterComponent(IEntity entity, MapMonsterDto dto)
        {
            Entity = entity;
            Vnum = dto.NpcMonsterId;
            MapNpcMonsterId = dto.Id;
            MapId = dto.MapId;
            IsAggressive = !dto.NpcMonster.NoAggresiveIcon;
        }

        public NpcMonsterComponent(IEntity entity, MapNpcDto dto)
        {
            Entity = entity;
            Vnum = dto.NpcMonsterId;
            MapNpcMonsterId = dto.Id;
            MapId = dto.MapId;
            IsAggressive = !dto.NpcMonster.NoAggresiveIcon;
        }

        public long Vnum { get; set; }
        public long MapNpcMonsterId { get; set; }
        public long MapId { get; set; }
        public bool IsAggressive { get; set; }

        public IEntity Entity { get; }
    }
}