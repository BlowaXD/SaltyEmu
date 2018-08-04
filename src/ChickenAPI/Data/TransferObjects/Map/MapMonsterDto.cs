using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Data.TransferObjects.NpcMonster;

namespace ChickenAPI.Game.Data.TransferObjects.Map
{
    public class MapMonsterDto : IMappedDto
    {
        public bool IsDisabled { get; set; }

        public bool IsMoving { get; set; }

        public short MapId { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public NpcMonsterDto NpcMonster { get; set; }

        public long NpcMonsterId { get; set; }

        public DirectionType Position { get; set; }
        public long Id { get; set; }
    }
}