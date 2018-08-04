using System.IO;

namespace ChickenAPI.Core.Plugins
{
    public interface IPluginManager
    {
        IPlugin[] LoadPlugin(FileInfo file);
        IPlugin[] LoadPlugins(DirectoryInfo directory);
    }
}