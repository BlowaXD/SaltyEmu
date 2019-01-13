namespace ChickenAPI.Core.Plugins.Exceptions
{
    /// <summary>
    ///     This exception should be thrown only if you need to stop the software
    /// </summary>
    public class CriticalPluginException : PluginException
    {
        public CriticalPluginException(IPlugin plugin, string message = "Critical Plugin Exception") : base($"[{plugin.Name}] {message}") => Plugin = plugin;

        public IPlugin Plugin { get; }
    }
}