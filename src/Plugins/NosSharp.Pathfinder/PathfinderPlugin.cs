using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Game.Maps;

namespace NosSharp.Pathfinder
{
    public class PathfinderPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<PathfinderPlugin>();
        public string Name => nameof(PathfinderPlugin);

        public void OnLoad()
        {
            Log.Info("Loading...");
            ChickenContainer.Builder.Register(s => new Pathfinder.Pathfinder()).As<IPathfinder>();
            Log.Info("Loaded !");
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