using System.Collections.Generic;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Features.Shops
{
    public class ShopComponent : IComponent
    {
        public ShopComponent(IEntity entity) => Entity = entity;

        public ISet<Shop> Shops { get; set; }

        public IEntity Entity { get; }
    }
}