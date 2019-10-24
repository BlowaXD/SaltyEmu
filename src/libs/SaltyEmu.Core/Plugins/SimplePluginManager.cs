using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using Logger = SaltyEmu.Core.Logging.Logger;

namespace SaltyEmu.Core.Plugins
{
    public class SimplePluginManager : IPluginManager
    {
        // private static readonly Logger Log = Logger.GetLogger<SimplePluginManager>();

        public IPlugin[] LoadPlugin(FileInfo file)
        {
            try
            {
                if (file == null)
                {
                    throw new ArgumentNullException(nameof(file));
                }

                Assembly assembly = Assembly.LoadFrom(file.FullName);

                if (assembly == null)
                {
                    return null;
                }

                Type[] types = assembly.GetTypes();
                Type pluginType = typeof(IPlugin);
                ICollection<Type> pluginTypes = types.Where(type => !type.IsInterface && !type.IsAbstract && type.GetInterface(pluginType.FullName) != null).ToArray();
                ICollection<IPlugin> plugins = new List<IPlugin>(pluginTypes.Count);
                foreach (Type type in pluginTypes)
                {
                    var plugin = (IPlugin)Activator.CreateInstance(type);
                    // Log.Info($"{plugin.Name} Loaded !");
                    plugin.OnLoad();
                    plugins.Add(plugin);
                }

                return plugins.ToArray();
            }
            catch
            {
                return null;
            }
        }

        public IPlugin[] LoadPlugins(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (directory.Exists)
            {
                return directory.GetFiles("*.dll").SelectMany(s =>
                {
                    IPlugin[] tmp = LoadPlugin(s);

                    return tmp;
                }).Where(s => s != null).ToArray();
            }

            directory.Create();
            return null;
        }

        public DirectoryInfo GetPluginDirectory() => new DirectoryInfo("plugins");

        public DirectoryInfo GetConfigDirectory() => new DirectoryInfo("config");
    }
}