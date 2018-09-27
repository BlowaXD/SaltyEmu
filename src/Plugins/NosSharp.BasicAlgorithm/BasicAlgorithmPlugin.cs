using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.AccessLayer.NpcMonster;

namespace NosSharp.BasicAlgorithm
{
    public class BasicAlgorithmPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<BasicAlgorithmPlugin>();
        public string Name => nameof(BasicAlgorithmPlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }

        public void OnLoad()
        {
            Log.Info("Loading...");
            ChickenContainer.Builder.Register(s => new AlgorithmService()).As<IAlgorithmService>().SingleInstance();
            ChickenContainer.Builder.Register(s => new NpcMonsterAlgorithmService()).As<INpcMonsterAlgorithmService>().SingleInstance();
            ChickenContainer.Builder.Register(s => new DamageAlgorithm()).As<IDamageAlgorithm>().SingleInstance();
            Log.Info("Algorithms initialized");
        }

        public void ReloadConfig()
        {
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