using System.Collections.Generic;
using ChickenAPI.Data.Relations;

namespace ChickenAPI.Game.Entities.Player
{
    public interface IRelationList
    {
        List<RelationDto> Relation { get; set; }
    }
}