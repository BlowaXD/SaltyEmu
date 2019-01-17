using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Relations;
using ChickenAPI.Enums.Game.Relations;
using NUnit.Framework;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.FriendsPlugin;
using SaltyEmu.FriendsPlugin.Services;

namespace SaltyEmu.RelationService.Tests
{
    public class RelationInvitationProcessing
    {
        private const long SenderId = 1;
        private const long TargetId = 2;
        private IRelationService _relationService;

        [OneTimeSetUp]
        public void Setup()
        {
            MqttClientConfigurationBuilder builder = new MqttClientConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("relations-client");

            // _relationService = new RelationServiceClient(builder);
            // ((MqttIpcClient<RelationServiceClient>)_relationService).InitializeAsync().GetAwaiter().GetResult();
        }

        [Test]
        public async Task Success_Accept_Pending_Friend_Invitation()
        {
            await _relationService.RelationInviteAsync(SenderId, TargetId, CharacterRelationType.Friend);

            IEnumerable<RelationInvitationDto> invitations = await _relationService.GetPendingInvitationByCharacterIdAsync(TargetId);
            Assert.IsNotNull(invitations);

            RelationInvitationDto invitation = invitations.FirstOrDefault(s => s.OwnerId == SenderId);
            Assert.IsNotNull(invitation);

            // accept the invitation
            List<RelationDto> relation = await _relationService.RelationInviteProcessAsync(invitation.Id, RelationInvitationProcessType.Accept);

            IEnumerable<RelationDto> relations = await _relationService.GetRelationListByCharacterIdAsync(SenderId);
            Assert.IsTrue(relations.Any(s => s.OwnerId == SenderId && s.TargetId == TargetId && s.Type == CharacterRelationType.Friend && relation.Any(c => c.Id == s.Id)));

            // invitations should be clear
            IEnumerable<RelationInvitationDto> invitationsEmpty = await _relationService.GetPendingInvitationByCharacterIdAsync(TargetId);
            Assert.IsTrue(!invitationsEmpty.Any());

            await _relationService.RelationRemoveAsync(relations);
        }

        [Test]
        public async Task Success_Refuse_Pending_Friend_Invitation()
        {
            await _relationService.RelationInviteAsync(SenderId, TargetId, CharacterRelationType.Friend);

            IEnumerable<RelationInvitationDto> invitations = await _relationService.GetPendingInvitationByCharacterIdAsync(TargetId);
            Assert.IsNotNull(invitations);

            RelationInvitationDto invitation = invitations.FirstOrDefault(s => s.OwnerId == SenderId);
            Assert.IsNotNull(invitation);

            // refuse the invitation
            List<RelationDto> relation = await _relationService.RelationInviteProcessAsync(invitation.Id, RelationInvitationProcessType.Refuse);

            IEnumerable<RelationDto> relations = await _relationService.GetRelationListByCharacterIdAsync(SenderId);

            // invitations should be clear
            IEnumerable<RelationInvitationDto> invitationsEmpty = await _relationService.GetPendingInvitationByCharacterIdAsync(TargetId);
            Assert.IsTrue(!invitationsEmpty.Any());

            await _relationService.RelationRemoveAsync(relations);
        }
    }
}