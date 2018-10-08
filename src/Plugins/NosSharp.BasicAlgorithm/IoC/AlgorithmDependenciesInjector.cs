using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.AccessLayer.NpcMonster;

namespace NosSharp.BasicAlgorithm
{
    public class AlgorithmDependenciesInjector
    {
        public static void InjectDependencies()
        {
            ChickenContainer.Builder.Register(s => new AlgorithmService()).As<IAlgorithmService>().SingleInstance();
            ChickenContainer.Builder.Register(s => new NpcMonsterAlgorithmService()).As<INpcMonsterAlgorithmService>().SingleInstance();
            ChickenContainer.Builder.Register(s => new DamageAlgorithm()).As<IDamageAlgorithm>().SingleInstance();
        }
    }
}