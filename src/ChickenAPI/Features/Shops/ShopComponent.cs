using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;

namespace ChickenAPI.Game.Features.Shops
{
    public class ShopComponent : IComponent
    {
        public ShopComponent(IEntity entity) => Entity = entity;

        public ISet<Shop> Shops { get; set; }

        public IEntity Entity { get; }
    }
}