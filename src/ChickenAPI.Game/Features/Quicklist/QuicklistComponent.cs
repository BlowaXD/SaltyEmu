using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Quicklist
{
    public class QuicklistComponent : IComponent
    {
        public QuicklistComponent(IEntity entity, IEnumerable<CharacterQuicklistDto> quicklist)
        {
            Entity = entity;
            Quicklist.AddRange(quicklist.OrderBy(s => s.Position));
        }

        public IEntity Entity { get; }

        public List<CharacterQuicklistDto> Quicklist { get; set; } = new List<CharacterQuicklistDto>();
    }
}