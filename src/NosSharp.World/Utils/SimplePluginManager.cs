using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ChickenAPI.Plugins;
using ChickenAPI.Utils;

namespace NosSharp.World.Utils
{
    public class SimplePluginManager : IPluginManager
    {
        public DirectoryInfo GetPluginDirectory() => new DirectoryInfo("plugin");
        public DirectoryInfo GetConfigDirectory() => new DirectoryInfo("plugin/config");

        public IPlugin LoadPlugin(FileInfo file)
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
                    plugins.Add(plugin);
                }

                return plugins.FirstOrDefault();
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
                return directory.GetFiles("*.dll").Select(s =>
                {
                    IPlugin tmp = LoadPlugin(s);
                    if (tmp == null)
                    {
                        return null;
                    }

                    Logger.Log.Info($"[PluginManager] Loading plugin {tmp.Name}...");
                    tmp.OnLoad();

                    return tmp;
                }).Where(s => s != null).ToArray();
            }

            directory.Create();
            return null;
        }
    }
}