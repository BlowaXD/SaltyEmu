using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Groups
{
    public class GroupComponent : IComponent
    {
        public GroupComponent(IEntity entity) => Entity = entity;

        public List<IPlayerEntity> Members { get; set; }

        public IEntity Entity { get; }
    }
}