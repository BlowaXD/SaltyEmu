using System.Collections.Generic;
using ChickenAPI.Data;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Groups
{
    public class GroupDto : IMappedDto
    {
        public IPlayerEntity Leader { get; set; }
        public List<IPlayerEntity> Players { get; set; }
        public long Id { get; set; }
    }
}