using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Map;
using ChickenAPI.Game.Data.TransferObjects.Shop;

namespace ChickenAPI.Game.Features.Shops
{
    public class ShopComponent : IComponent
    {
        public ShopComponent(IEntity entity, MapNpcDto npc, ShopDto dto) => Entity = entity;

        public long Id { get; set; }

        public long MapNpcId { get; set; }

        public string Name { get; set; }

        public byte MenuType { get; set; }

        public byte ShopType { get; set; }

        public IEntity Entity { get; }
    }
}