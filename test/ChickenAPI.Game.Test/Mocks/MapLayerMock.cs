using ChickenAPI.Data.Map;
using ChickenAPI.Game.Maps;

namespace ChickenAPI.Game.Test.Mocks
{
    public class MapLayerMock : SimpleMapLayer
    {
        public MapLayerMock() : base(
            new SimpleMap(new MapDto { Id = 1, Grid = new byte[] { }, Name = "", Height = 100, Width = 100 },
            null, null, null, null, false), null, null, null, null, false)
        {
        }
    }
}