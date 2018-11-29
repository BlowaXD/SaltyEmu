namespace ChickenAPI.Game.Chat.Events
{
    public class ChatWhisperEvent
    {
        public string TargetName { get; set; }
        public string Message { get; set; }
    }
}