using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.Data.TransferObjects.Shop;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;

namespace ChickenAPI.Game.Features.Shops
{
    public class ShopComponent : IComponent
    {
        public ShopComponent(IEntity entity, MapNpcDto npc, ShopDto dto)
        {
            Entity = entity;
        }

        public long Id { get; set; }

        public long MapNpcId { get; set; }

        public string Name { get; set; }

        public byte MenuType { get; set; }

        public byte ShopType { get; set; }

        public IEntity Entity { get; }
    }
}