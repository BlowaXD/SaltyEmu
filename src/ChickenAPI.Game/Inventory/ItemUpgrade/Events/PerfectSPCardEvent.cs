using ChickenAPI.Data.Item;
using ChickenAPI.Game._Events;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Events
{
    public class PerfectSPCardEvent : GameEntityEvent
    {
        public ItemInstanceDto SpCard { get; set; }

        public int UpMode { get; set; }

        public int StoneVnum { get; set; }
    }
}