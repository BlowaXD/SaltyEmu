using System.IO;
using Newtonsoft.Json;

namespace ChickenAPI.Core.Utils
{
    public class ConfigurationHelper
    {
        public static T Load<T>(string path) where T : class, new() => Load<T>(path, false);

        public static T Load<T>(string path, bool createIfNotExists) where T : class, new()
        {
            if (!File.Exists(path))
            {
                if (createIfNotExists)
                {
                    Save(path, new T());
                }
                else
                {
                    throw new IOException(path);
                }
            }

            string fileContent = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(fileContent);
        }

        public static void Save<T>(string path, T value)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            string valueSerialized = JsonConvert.SerializeObject(value, Formatting.Indented);

            File.WriteAllText(path, valueSerialized);
        }
    }
}