using System.Collections.Generic;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;

namespace ChickenAPI.Game.Shops
{
    public class ShopComponent : IComponent
    {
        public ShopComponent(IEntity entity) => Entity = entity;

        public ISet<Shop> Shops { get; set; }

        public IEntity Entity { get; }
    }
}