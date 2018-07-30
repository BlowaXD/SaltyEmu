using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.AccessLayer;
using ChickenAPI.Data.AccessLayer.Character;
using ChickenAPI.Data.AccessLayer.NpcMonster;
using ChickenAPI.Plugins;

namespace NosSharp.BasicAlgorithm
{
    public class BasicAlgorithmPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<BasicAlgorithmPlugin>();
        public string Name => nameof(BasicAlgorithmPlugin);

        public void OnDisable()
        {
            return;
        }

        public void OnEnable()
        {
            return;
        }

        public void OnLoad()
        {
            Log.Info("Loading...");
            Container.Builder.Register(s => new AlgorithmService()).As<IAlgorithmService>().SingleInstance();
            Container.Builder.Register(s => new NpcMonsterAlgorithmService()).As<INpcMonsterAlgorithmService>().SingleInstance();
            Log.Info("Algorithms initialized");
        }

        public void ReloadConfig()
        {
            return;
        }

        public void SaveConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveDefaultConfig()
        {
            throw new NotImplementedException();
        }
    }
}