using ChickenAPI.Game.Quicklist;

namespace ChickenAPI.Game.Entities
{
    public interface IQuicklistEntity
    {
        QuicklistComponent Quicklist { get; }
    }
}