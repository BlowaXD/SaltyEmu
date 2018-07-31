using System.IO;

namespace ChickenAPI.Plugins
{
    public interface IPluginManager
    {
        IPlugin[] LoadPlugin(FileInfo file);
        IPlugin[] LoadPlugins(DirectoryInfo directory);
    }
}