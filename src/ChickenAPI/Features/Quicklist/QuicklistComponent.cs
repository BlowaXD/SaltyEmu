using System.Collections.Generic;
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
            foreach (CharacterQuicklistDto i in quicklist)
            {
                if (i.IsQ1)
                {
                    Q1.Add(i.Position, i);
                }
                else
                {
                    Q2.Add(i.Position, i);
                }
            }
        }

        public IEntity Entity { get; }

        public List<CharacterQuicklistDto> Quicklist { get; set; } = new List<CharacterQuicklistDto>();
        public Dictionary<long, CharacterQuicklistDto> Q1 { get; } = new Dictionary<long, CharacterQuicklistDto>();
        public Dictionary<long, CharacterQuicklistDto> Q2 { get; } = new Dictionary<long, CharacterQuicklistDto>();
    }
}