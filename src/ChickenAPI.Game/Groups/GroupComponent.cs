using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Groups
{
    public class GroupComponent : IComponent
    {
        public GroupComponent(IEntity entity) => Entity = entity;

        public List<IPlayerEntity> Members { get; set; }

        public IEntity Entity { get; }
    }
}