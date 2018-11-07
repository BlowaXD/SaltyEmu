using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Test.Mocks;
using Microsoft.EntityFrameworkCore;
using NosSharp.BasicAlgorithm;
using SaltyEmu.BasicPlugin;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Utils;
using SaltyEmu.PathfinderPlugin.Algorithms;

namespace ChickenAPI.Game.Test
{
    public static class TestHelper
    {
        private static bool _initialized;
        private static ICharacterService _characterService;
        private static IPlayerManager _playerManager;
        private static MapLayerMock Layer;

        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            InjectDependencies();
            ChickenContainer.Initialize();
            LoadDatabase();
        }

        private static void LoadDatabase()
        {
            _characterService = ChickenContainer.Instance.Resolve<ICharacterService>();
            Layer = new MapLayerMock();
            _playerManager = ChickenContainer.Instance.Resolve<IPlayerManager>();
        }

        private static void InjectDependencies()
        {
            ChickenContainer.Builder.Register(s =>
            {
                DbContextOptionsBuilder<SaltyDbContext> options = new DbContextOptionsBuilder<SaltyDbContext>().UseInMemoryDatabase("test");
                return new SaltyDbContext(options.Options);
            }).As<SaltyDbContext>().InstancePerDependency();
            PluginDependencyInjector.RegisterMapping();
            PluginDependencyInjector.RegisterDaos();
            AlgorithmDependenciesInjector.InjectDependencies();
            BasicPluginIoCInjector.InjectDependencies();
            ChickenContainer.Builder.Register(s => new Pathfinder()).As<IPathfinder>();
        }

        public static PlayerEntity LoadPlayer(string name)
        {
            CharacterDto dto = _characterService.GetActiveByNameAsync(name).Result;
            if (dto != null)
            {
                return new PlayerEntity(new SessionMock(), dto, null, null);
            }

            CharacterDto newCharacter = _characterService.GetCreationCharacter();

            newCharacter.Class = CharacterClassType.Adventurer;
            newCharacter.Gender = GenderType.Male;
            newCharacter.HairColor = HairColorType.Black;
            newCharacter.HairStyle = HairStyleType.HairStyleA;
            newCharacter.Name = name;
            newCharacter.Slot = 1;
            newCharacter.AccountId = 1;
            newCharacter.MinilandMessage = "Welcome";
            newCharacter.State = CharacterState.Active;
            dto = _characterService.Save(newCharacter);
            var tmp = new PlayerEntity(new SessionMock(), dto, null, null);
            tmp.TransferEntity(Layer);
            _playerManager.RegisterPlayer(tmp);
            return tmp;
        }
    }
}