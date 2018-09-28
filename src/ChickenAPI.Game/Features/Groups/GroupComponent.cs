using System.Collections.Generic;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
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