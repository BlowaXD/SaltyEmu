namespace SaltyEmu.FriendsPlugin
{
    public class Configuration
    {
        public const string Prefix = "/relations";

        public const string RequestQueue = Prefix + "/" + "request";
        public const string ResponseQueue = Prefix + "/" + "response";
        public const string BroadcastQueue = Prefix + "/" + "broadcast";
    }
}