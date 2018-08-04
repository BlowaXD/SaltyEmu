using System.IO;

namespace ChickenAPI.Core.Plugins
{
    public interface IPluginConfiguration
    {
        void Load(FileInfo file);
        void Load(string filePath);

        void Save(FileInfo file);
        void Save(string filePath);
    }
}