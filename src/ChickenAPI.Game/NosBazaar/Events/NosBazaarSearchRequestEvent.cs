using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.NosBazaar.Events
{
    public class NosBazaarSearchRequestEvent : ChickenEventArgs
    {
        public string SearchString { get; set; }
        // add filters
    }
}