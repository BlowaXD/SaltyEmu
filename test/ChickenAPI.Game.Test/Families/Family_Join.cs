using System.Linq;
using ChickenAPI.Enums.Game.Families;
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
        private readonly BasicFamilyEventHandler _familyEventHandler = new BasicFamilyEventHandler();

        [OneTimeSetUp]
        public void Setup()
        {
            TestHelper.Initialize();
        }

        private static IPlayerEntity LoadPlayer(string name) => TestHelper.LoadPlayer(name);

        [Test]
        public void Family_Join_Success_Existing_Family()
        {
            const string familyName = "family_join_test";
            IPlayerEntity leader = LoadPlayer("test_leader");
            IPlayerEntity newPlayer = LoadPlayer("test_member");

            _familyEventHandler.Execute(leader, new FamilyCreationEvent
            {
                Leader = leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = leader.Family,
            });

            Assert.AreEqual(leader.Family, newPlayer.Family);
            Assert.AreEqual(leader.Family.Id, newPlayer.FamilyCharacter.FamilyId);
            Assert.AreEqual(newPlayer.Character.Id, newPlayer.FamilyCharacter.CharacterId);
            Assert.AreEqual(newPlayer.FamilyCharacter.Authority, FamilyAuthority.Member);
            Assert.IsTrue(((SessionMock)newPlayer.Session).Packets.Any(s => s.Item1 == typeof(GidxPacket)));
            Assert.IsTrue(((SessionMock)newPlayer.Session).Packets.Any(s => s.Item1 == typeof(GInfoPacket)));
        }

        [Test]
        public void Family_Join_Success_Custom_Role()
        {
            const string familyName = "family_join_test_custom_role";
            IPlayerEntity leader = LoadPlayer("test_leader_custom_role");
            IPlayerEntity newPlayer = LoadPlayer("test_member_custom_role");

            _familyEventHandler.Execute(leader, new FamilyCreationEvent
            {
                Leader = leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = leader.Family,
                ExpectedAuthority = FamilyAuthority.Assistant
            });

            Assert.AreEqual(leader.Family, newPlayer.Family);
            Assert.AreEqual(leader.Family.Id, newPlayer.FamilyCharacter.FamilyId);
            Assert.AreEqual(newPlayer.Character.Id, newPlayer.FamilyCharacter.CharacterId);
            Assert.AreEqual(newPlayer.FamilyCharacter.Authority, FamilyAuthority.Assistant);
            Assert.IsTrue(((SessionMock)newPlayer.Session).Packets.Any(s => s.Item1 == typeof(GidxPacket)));
            Assert.IsTrue(((SessionMock)newPlayer.Session).Packets.Any(s => s.Item1 == typeof(GInfoPacket)));
        }

        [Test]
        public void Family_Join_Fail_Custom_Role_Two_Leaders()
        {
            const string familyName = "family_join_test_two_leaders";
            IPlayerEntity leader = LoadPlayer("test_leader_two_leaders");
            IPlayerEntity newPlayer = LoadPlayer("test_member_two_leaders");

            _familyEventHandler.Execute(leader, new FamilyCreationEvent
            {
                Leader = leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = leader.Family,
                ExpectedAuthority = FamilyAuthority.Head
            });

            Assert.IsFalse(newPlayer.HasFamily);
            Assert.IsNull(newPlayer.FamilyCharacter);
        }

        [Test]
        public void Family_Join_Fail_Family_Does_Not_Exist()
        {
            const string familyName = "family_join_test_no_family";
            IPlayerEntity leader = LoadPlayer("test_leader_no_family");
            IPlayerEntity newPlayer = LoadPlayer("test_member_no_family");

            _familyEventHandler.Execute(leader, new FamilyCreationEvent
            {
                Leader = leader,
                FamilyName = familyName
            });
            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = null,
                ExpectedAuthority = FamilyAuthority.Member
            });

            Assert.IsFalse(newPlayer.HasFamily);
        }

        [Test]
        public void Family_Join_Fail_Already_In_Family()
        {
            // leader
            IPlayerEntity leader = LoadPlayer("test_leader_already_in_family");
            const string familyName = "family_join_test_already_in_family";

            _familyEventHandler.Execute(leader, new FamilyCreationEvent
            {
                Leader = leader,
                FamilyName = familyName
            });

            // member
            IPlayerEntity newPlayer = LoadPlayer("test_member_already_in_family");
            const string newPlayerFamily = "family_join_test_already_in_family_newplayer";
            _familyEventHandler.Execute(newPlayer, new FamilyCreationEvent
            {
                Leader = newPlayer,
                FamilyName = newPlayerFamily
            });


            _familyEventHandler.Execute(newPlayer, new FamilyJoinEvent
            {
                Player = newPlayer,
                Family = leader.Family,
                ExpectedAuthority = FamilyAuthority.Member
            });

            Assert.IsTrue(newPlayer.HasFamily);
            Assert.IsTrue(newPlayer.IsFamilyLeader);
            Assert.IsTrue(leader.HasFamily);
            Assert.IsTrue(leader.IsFamilyLeader);
            Assert.AreNotEqual(newPlayer.Family.Id, leader.Family.Id);
        }
    }
}