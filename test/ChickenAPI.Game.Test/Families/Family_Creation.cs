using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Families;
using ChickenAPI.Game.Families.Events;
using NUnit.Framework;

namespace ChickenAPI.Game.Test.Families
{
    [TestFixture]
    public class Family_Creation
    {
        private ICharacterService _characterService;
        private readonly BasicFamilyEventHandler _familyEventHandler = new BasicFamilyEventHandler();

        [OneTimeSetUp]
        public void Setup()
        {
            TestHelper.Initialize();
            _characterService = ChickenContainer.Instance.Resolve<ICharacterService>();
        }

        private static IPlayerEntity LoadPlayer(string name)
        {
            return TestHelper.LoadPlayer(name);
        }

        [Test]
        public void Family_Creation_Success_Only_Leader()
        {
            string familyName = "family_name_test";
            IPlayerEntity player = LoadPlayer("test_only_leader");

            Assert.IsFalse(player.HasFamily);
            Assert.IsFalse(player.IsFamilyLeader);
            Assert.IsNull(player.Family);
            Assert.IsNull(player.FamilyCharacter);

            _familyEventHandler.Execute(player, new FamilyCreationEvent
            {
                Leader = player,
                FamilyName = familyName
            });

            Assert.IsTrue(player.HasFamily);
            Assert.IsTrue(player.IsFamilyLeader);
            Assert.IsNotNull(player.Family);
            Assert.IsNotNull(player.FamilyCharacter);
            Assert.AreEqual(player.Family.Id, player.FamilyCharacter.FamilyId);
            Assert.AreEqual(player.Family.Name, familyName);
        }

        [Test]
        public void Family_Creation_Success_Three_Assistants()
        {
            string familyName = "family_name_test_three_assistants";

            IPlayerEntity player = LoadPlayer("test_three_persons");
            IPlayerEntity firstAssist = LoadPlayer("player_assist_1");
            IPlayerEntity secondAssist = LoadPlayer("player_assist_2");
            IPlayerEntity thirdAssist = LoadPlayer("player_assist_3");

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
                Assert.IsTrue(assistant.HasFamily);
                Assert.IsFalse(assistant.IsFamilyLeader);
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

            Assert.IsNull(player.Family);
            Assert.IsNull(player.FamilyCharacter);
            Assert.IsFalse(player.HasFamily);
            Assert.IsFalse(player.IsFamilyLeader);
        }

        [Test]
        public void Family_Creation_Fail_Already_in_Family()
        {
            const string familyName = "family_name_test_fail_same_name";
            const string secondFamily = "family_second";

            IPlayerEntity player = LoadPlayer("test_fail_already_in_family");
            _familyEventHandler.Execute(player, new FamilyCreationEvent
            {
                Leader = player,
                FamilyName = familyName,
            });

            Assert.IsNotNull(player.Family);
            Assert.IsNotNull(player.FamilyCharacter);
            long familyId = player.Family.Id;

            // remove family from
            _familyEventHandler.Execute(player, new FamilyCreationEvent
            {
                Leader = player,
                FamilyName = secondFamily,
            });

            Assert.IsNotNull(player.Family);
            Assert.IsNotNull(player.FamilyCharacter);
            Assert.AreEqual(player.Family.Name, familyName);
            Assert.AreEqual(player.Family.Id, familyId);
        }
    }
}