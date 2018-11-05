namespace ChickenAPI.Game.Features.Shops
{
    public interface IShopCapacity
    {
        /// <summary>
        /// Tells whether or not the given object has a shop
        /// </summary>
        bool HasShop { get; }

        /// <summary>
        /// Shop access
        /// </summary>
        Shop Shop { get; }
    }
}