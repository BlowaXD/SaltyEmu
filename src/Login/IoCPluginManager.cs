using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;

namespace Login
{
    public class IoCPluginManager
    {
        private readonly ILogger _log;

        public IoCPluginManager(ILogger log)
        {
            _log = log;
        }

        private bool PluginMatcher(Type type)
        {
            return !type.IsInterface && !type.IsAbstract && type.GetInterface(typeof(IPlugin).FullName) != null;
        }

        public void RegisterPlugins(Assembly assembly, ContainerBuilder builder)
        {
            builder.RegisterTypes(assembly.GetTypes().Where(PluginMatcher).ToArray()).AsImplementedInterfaces().AsSelf().InstancePerDependency();
        }

        public void RegisterPlugins(FileInfo file, ContainerBuilder builder)
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
                    return;
                }

                RegisterPlugins(assembly, builder);
            }
            catch (Exception e)
            {
                _log.Error("Register plugins", e);
            }
        }

        public void RegisterPlugins(DirectoryInfo directory, ContainerBuilder builder)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (directory.Exists)
            {
                foreach (FileInfo dynamicLibrary in directory.GetFiles("*.dll"))
                {
                    RegisterPlugins(dynamicLibrary, builder);
                }
            }

            directory.Create();
        }
    }
}