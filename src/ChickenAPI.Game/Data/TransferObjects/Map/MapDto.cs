using ChickenAPI.Core.Data.TransferObjects;

namespace ChickenAPI.Game.Data.TransferObjects.Map
{
    public class MapDto : IMappedDto
    {
        public string Name { get; set; }
        public bool AllowShop { get; set; }
        public bool AllowPvp { get; set; }
        public int Music { get; set; }
        public short Height { get; set; }
        public short Width { get; set; }
        public byte[] Grid { get; set; }
        public long Id { get; set; }
    }
}