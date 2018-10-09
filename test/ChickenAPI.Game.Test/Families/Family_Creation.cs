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
using ChickenAPI.Game.Families;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Test.Mocks;
using Microsoft.EntityFrameworkCore;
using NosSharp.BasicAlgorithm;
using NUnit.Framework;
using SaltyEmu.BasicPlugin;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Utils;

namespace ChickenAPI.Game.Test.Families
{
    [TestFixture]
    public class Family_Creation
    {
        private bool _initialized;
        private ICharacterService _characterService;
        private readonly BasicFamilyEventHandler _familyEventHandler = new BasicFamilyEventHandler();

        [SetUp]
        public void Setup()
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

        PlayerEntity LoadPlayer(string name)
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

            return new PlayerEntity(new SessionMock(), dto, null, null);
        }

        [Test]
        public void Family_Creation_Success_Only_Leader()
        {
            string familyName = "family_name_test";
            IPlayerEntity player = LoadPlayer("test_only_leader");
            Assert.IsNull(player.Family);
            Assert.IsNull(player.FamilyCharacter);

            _familyEventHandler.Execute(player, new FamilyCreationEvent
            {
                Leader = player,
                FamilyName = familyName
            });

            Assert.IsNotNull(player.Family);
            Assert.IsNotNull(player.FamilyCharacter);
            Assert.AreEqual(player.Family.Id, player.FamilyCharacter.FamilyId);
            Assert.AreEqual(player.Family.Name, familyName);
        }

        [Test]
        public void Family_Creation_Success_Three_Persons()
        {
            string familyName = "family_name_test_three_persons";

            IPlayerEntity player = LoadPlayer("test_three_persons");
            PlayerEntity firstAssist = LoadPlayer("player_assist_1");
            PlayerEntity secondAssist = LoadPlayer("player_assist_2");
            PlayerEntity thirdAssist = LoadPlayer("player_assist_3");

            List<IPlayerEntity> assistants = new List<IPlayerEntity>
            {
                firstAssist,
                secondAssist,
                thirdAssist
            };

            foreach (IPlayerEntity assistant in assistants)
            {
                Assert.IsNull(assistant.Family);
                Assert.IsNull(assistant.FamilyCharacter);
            }

            _familyEventHandler.Execute(player, new FamilyCreationEvent
            {
                Leader = player,
                FamilyName = familyName,
                Assistants = assistants
            });

            foreach (IPlayerEntity assistant in assistants)
            {
                Assert.IsNotNull(assistant.Family);
                Assert.IsNotNull(assistant.FamilyCharacter);
                Assert.AreEqual(assistant.Family, player.Family);
                Assert.AreEqual(assistant.Family.Name, familyName);
                Assert.AreEqual(assistant.FamilyCharacter.CharacterId, assistant.Character.Id);
            }
        }

        [Test]
        public void Family_Creation_Fail_Family_Name_Already_Taken()
        {
            string familyName = "family_name_test_fail_same_name";

            IPlayerEntity player = LoadPlayer("test_fail_name_already_taken");
            _familyEventHandler.Execute(player, new FamilyCreationEvent
            {
                Leader = player,
                FamilyName = familyName,
            });

            Assert.IsNotNull(player.Family);
            Assert.IsNotNull(player.FamilyCharacter);

            // remove family from
            player.Family = null;
            player.FamilyCharacter = null;
            _familyEventHandler.Execute(player, new FamilyCreationEvent
            {
                Leader = player,
                FamilyName = familyName,
            });

            Assert.IsNull(player.Family);
            Assert.IsNull(player.FamilyCharacter);
        }
    }
}