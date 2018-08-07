namespace NosSharp.RedisSessionPlugin
{
    public class RedisConfiguration
    {
        public RedisConfiguration()
        {
            Host = "localhost";
            Port = 6379;
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}