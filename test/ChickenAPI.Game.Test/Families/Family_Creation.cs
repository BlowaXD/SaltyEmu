using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Test.Mocks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NosSharp.BasicAlgorithm;
using NUnit.Framework;
using SaltyEmu.BasicPlugin;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Services.Account;
using SaltyEmu.DatabasePlugin.Utils;

namespace ChickenAPI.Game.Test.Families
{
    [TestFixture]
    public class Family_Creation
    {
        private ICharacterService _characterService;
        private IPlayerEntity _player;

        [SetUp]
        public void Setup()
        {
            InjectDependencies();
            ChickenContainer.Initialize();
            LoadDatabase();
            LoadPlayer();
            InitializeEventHandlers();
        }

        public void LoadDatabase()
        {
            _characterService = ChickenContainer.Instance.Resolve<ICharacterService>();
        }

        private void InjectDependencies()
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
        }

        private void LoadPlayer()
        {
            CharacterDto dto = _characterService.GetActiveByNameAsync("test_player_1").Result;
            if (dto == null)
            {
                CharacterDto newCharacter = _characterService.GetCreationCharacter();

                newCharacter.Class = CharacterClassType.Adventurer;
                newCharacter.Gender = GenderType.Male;
                newCharacter.HairColor = HairColorType.Black;
                newCharacter.HairStyle = HairStyleType.HairStyleA;
                newCharacter.Name = "test_player_1";
                newCharacter.Slot = 1;
                newCharacter.AccountId = 1;
                newCharacter.MinilandMessage = "Welcome";
                newCharacter.State = CharacterState.Active;
                dto = _characterService.Save(newCharacter);
            }

            _player = new PlayerEntity(new SessionMock(), dto, null, null);
        }

        private static IEnumerable<Type> GetHandlers()
        {
            List<Type> handlers = new List<Type>();

            handlers.AddRange(typeof(EffectEventHandler).Assembly.GetTypes().Where(s => s.IsSubclassOf(typeof(EventHandlerBase))));
            return handlers;
        }

        private static void InitializeEventHandlers()
        {
            // first version hardcoded, next one through Plugin + Assembly Reflection
            var eventManager = ChickenContainer.Instance.Resolve<IEventManager>();
            foreach (Type handlerType in GetHandlers())
            {
                object handler = Activator.CreateInstance(handlerType);

                if (!(handler is EventHandlerBase handlerBase))
                {
                    continue;
                }

                foreach (Type type in handlerBase.HandledTypes)
                {

                    eventManager.Register(handlerBase, type);
                }

            }
        }

        [Test]
        public void Family_Creation_Success_Only_Leader()
        {
            string familyName = "family_name_test";
            _player.EmitEvent(new FamilyCreationEvent
            {
                Leader = _player,
                FamilyName = familyName
            });

            Assert.IsNotNull(_player.Family);
            Assert.IsNotNull(_player.FamilyCharacter);
            Assert.AreEqual(_player.Family.Id, _player.FamilyCharacter.FamilyId);
            Assert.AreEqual(_player.Family.Name, familyName);
        }
    }
}