using System.Collections.Generic;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Families;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Test.Mocks;
using ChickenAPI.Packets.Game.Client.Families;
using NUnit.Framework;

namespace ChickenAPI.Game.Test.Families
{
    [TestFixture]
    public class Family_Join
    {
        private IPlayerEntity _leader;
        private ICharacterService _characterService;
        private readonly BasicFamilyEventHandler _familyEventHandler = new BasicFamilyEventHandler();

        const string familyName = "family_join_test";
        
        [SetUp]
        public void Setup()
        {
            TestHelper.Initialize();
            _characterService = ChickenContainer.Instance.Resolve<ICharacterService>();
            _leader = LoadPlayer("test_leader");
        }

        private static IPlayerEntity LoadPlayer(string name) => TestHelper.LoadPlayer(name);

        [Test]
        public void Family_Join_Success_Existing_Family()
        {
            IPlayerEntity newPlayer = LoadPlayer("test_member");

            _familyEventHandler.Execute(_leader, new FamilyCreationEvent
            {
                Leader = _leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = _leader.Family,
            });

            Assert.AreEqual(_leader.Family, newPlayer.Family);
            Assert.AreEqual(_leader.Family.Id, newPlayer.FamilyCharacter.FamilyId);
            Assert.AreEqual(newPlayer.Character.Id, newPlayer.FamilyCharacter.CharacterId);
            Assert.AreEqual(newPlayer.FamilyCharacter.Authority, FamilyAuthority.Member);
        }

        [Test]
        public void Family_Join_Success_Custom_Role()
        {
            IPlayerEntity newPlayer = LoadPlayer("test_member_custom_role");

            _familyEventHandler.Execute(_leader, new FamilyCreationEvent
            {
                Leader = _leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = _leader.Family,
                ExpectedAuthority = FamilyAuthority.Assistant
            });

            Assert.AreEqual(_leader.Family, newPlayer.Family);
            Assert.AreEqual(newPlayer.FamilyCharacter.FamilyId, newPlayer.FamilyCharacter.FamilyId);
            Assert.AreEqual(newPlayer.Character.Id, newPlayer.FamilyCharacter.CharacterId);
            Assert.AreEqual(newPlayer.FamilyCharacter.Authority, FamilyAuthority.Assistant);
            Assert.IsTrue(((SessionMock)newPlayer.Session).Packets.Any(s => s.Item1 == typeof(GidxPacket)));
            Assert.IsTrue(((SessionMock)newPlayer.Session).Packets.Any(s => s.Item1 == typeof(GidxPacket)));
        }

        [Test]
        public void Family_Join_Fail_Custom_Role_Two_Leaders()
        {
            IPlayerEntity newPlayer = LoadPlayer("test_member_fail_already_leader");
            _familyEventHandler.Execute(_leader, new FamilyCreationEvent
            {
                Leader = _leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = _leader.Family,
                ExpectedAuthority = FamilyAuthority.Head
            });

            Assert.IsNotNull(_leader.Family);
            Assert.IsTrue(_leader.IsFamilyLeader);
            Assert.IsNull(newPlayer.Family);
            Assert.IsNull(newPlayer.FamilyCharacter);
        }

        [Test]
        public void Family_Join_Fail_Family_Does_Not_Exist()
        {
            IPlayerEntity newPlayer = LoadPlayer("test_member_fail_no_family");
            _familyEventHandler.Execute(_leader, new FamilyCreationEvent
            {
                Leader = _leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = null,
                ExpectedAuthority = FamilyAuthority.Member
            });

            Assert.AreEqual(_leader.Family, newPlayer.Family);
            Assert.AreEqual(_leader.Family.Id, newPlayer.FamilyCharacter.FamilyId);
            Assert.AreEqual(newPlayer.Character.Id, newPlayer.FamilyCharacter.CharacterId);
            Assert.AreEqual(newPlayer.FamilyCharacter.Authority, FamilyAuthority.Member);
        }

        [Test]
        public void Family_Join_Fail_Already_In_Family()
        {
            IPlayerEntity newPlayer = LoadPlayer("test_member_fail_already");
            _familyEventHandler.Execute(_leader, new FamilyCreationEvent
            {
                Leader = _leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = _leader.Family,
                ExpectedAuthority = FamilyAuthority.Member
            });

            Assert.AreEqual(_leader.Family, newPlayer.Family);
            Assert.AreEqual(_leader.Family.Id, newPlayer.FamilyCharacter.FamilyId);
            Assert.AreEqual(newPlayer.Character.Id, newPlayer.FamilyCharacter.CharacterId);
            Assert.AreEqual(newPlayer.FamilyCharacter.Authority, FamilyAuthority.Member);
        }
    }
}