using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ChickenAPI.Plugin;

namespace NosSharp.World.Utils
{
    public class SimplePluginManager : IPluginManager
    {
        public IPlugin LoadPlugin(FileInfo file)
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
            ICollection<Type> pluginTypes = types.Where(type => !type.IsInterface && !type.IsAbstract).Where(type => type.GetInterface(pluginType.FullName, true) != null).ToArray();
            ICollection<IPlugin> plugins = new List<IPlugin>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                var plugin = (IPlugin)Activator.CreateInstance(type);
                plugins.Add(plugin);
            }

            return plugins.FirstOrDefault();
        }

        public IPlugin[] LoadPlugins(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (directory.Exists)
            {
                return directory.GetFiles("*.dll").Select(LoadPlugin).ToArray();
            }

            directory.Create();
            return null;

        }
    }
}