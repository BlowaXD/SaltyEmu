using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Enums.Game.Entity;

namespace ChickenAPI.Data.Map
{
    public class MapNpcDto : IMappedDto
    {
        public short Dialog { get; set; }

        public short Effect { get; set; }

        public short EffectDelay { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsMoving { get; set; }

        public bool IsSitting { get; set; }

        public short MapId { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public NpcMonsterDto NpcMonster { get; set; }

        public long NpcMonsterId { get; set; }

        public DirectionType Position { get; set; }
        public long Id { get; set; }
    }
}