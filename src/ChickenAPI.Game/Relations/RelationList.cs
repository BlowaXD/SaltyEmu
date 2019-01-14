using System.Collections.Generic;
using ChickenAPI.Data.Relations;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Relations
{
    public class RelationList : IRelationList
    {
        public List<RelationDto> Relation { get; set; } = new List<RelationDto>();
    }
}