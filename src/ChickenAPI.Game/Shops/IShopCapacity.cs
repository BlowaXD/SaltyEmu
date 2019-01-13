namespace ChickenAPI.Game.Shops
{
    public interface IShopCapacity
    {
        /// <summary>
        ///     Tells whether or not the given object has a shop
        /// </summary>
        bool HasShop { get; }
    }
}