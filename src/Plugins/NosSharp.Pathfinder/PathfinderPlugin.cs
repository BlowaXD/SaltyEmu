using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Logging;
using System;
using ChickenAPI.Core.IoC;
using Autofac;
using ChickenAPI.Game.Maps;
using NosSharp.Pathfinder.Pathfinder;

namespace NosSharp.Pathfinder
{
    public class PathfinderPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<PathfinderPlugin>();
        public string Name => nameof(PathfinderPlugin);

        public void OnLoad()
        {
            Log.Info($"Loading...");
            ChickenContainer.Builder.Register(s => new Pathfinder.Pathfinder()).As<IPathfinder>();
            Log.Info($"Loaded !");
        }

        public void ReloadConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveDefaultConfig()
        {
            throw new NotImplementedException();
        }


        public void OnDisable()
        {
        }

        public void OnEnable()
        { 
        }
    }
}