using System.IO;

namespace ChickenAPI.Plugins
{
    public interface IPluginConfiguration
    {
        void Load(FileInfo file);
        void Load(string filePath);

        void Save(FileInfo file);
        void Save(string filePath);
    }
}