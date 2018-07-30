using System.Collections.Generic;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.BCard;
using ChickenAPI.Data.TransferObjects.Drop;
using ChickenAPI.Data.TransferObjects.NpcMonster;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Utils;

namespace ChickenAPI.Data.TransferObjects.Map
{
    public class MapMonsterDto : IMappedDto
    {
        public long Id { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsMoving { get; set; }

        public short MapId { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public NpcMonsterDto NpcMonster { get; set; }

        public long NpcMonsterId { get; set; }

        public DirectionType Position { get; set; }
    }
}