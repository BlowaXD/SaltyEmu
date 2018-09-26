using ChickenAPI.Game.Features.Quicklist;

namespace ChickenAPI.Game.Entities
{
    public interface IQuicklistEntity
    {
        QuicklistComponent Quicklist { get; }
    }
}