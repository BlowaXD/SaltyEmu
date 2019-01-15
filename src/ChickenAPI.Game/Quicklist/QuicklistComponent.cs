using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Data.Character;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Quicklist
{
    public class QuicklistComponent : IComponent
    {
        public QuicklistComponent(IEntity entity, IEnumerable<CharacterQuicklistDto> quicklist)
        {
            Entity = entity;
            if (quicklist == null)
            {
                return;
            }

            Quicklist.AddRange(quicklist.OrderBy(s => s.Position));
        }

        public List<CharacterQuicklistDto> Quicklist { get; set; } = new List<CharacterQuicklistDto>();

        public IEntity Entity { get; }
    }
}