using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.i18n;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Relations.Events;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Old.Game.Server.Relations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Relations
{
    public class Relation_InvitationReceive_Handler : GenericEventPostProcessorBase<RelationInvitationReceiveEvent>
    {
        private readonly IPlayerManager _playerManager;
        private readonly ICharacterService _characterService;

        public Relation_InvitationReceive_Handler(ICharacterService characterService, IPlayerManager playerManager)
        {
            _characterService = characterService;
            _playerManager = playerManager;
        }

        protected override async Task Handle(RelationInvitationReceiveEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            CharacterDto senderInfo;
            IPlayerEntity target = _playerManager.GetPlayerByCharacterId(e.Invitation.OwnerId);

            if (target == null)
            {
                senderInfo = await _characterService.GetByIdAsync(e.Invitation.OwnerId);
                if (senderInfo == null)
                {
                    return;
                }
            }
            else
            {
                senderInfo = target.Character;
            }

            PacketBase acceptPacket = new FInsPacket { CharacterId = e.Invitation.OwnerId, Type = FInsPacketType.Accept };
            PacketBase refusePacket = new FInsPacket { CharacterId = e.Invitation.OwnerId, Type = FInsPacketType.Refuse };
            string question = player.GetLanguageFormat(PlayerMessages.FRIEND_X_INVITED_YOU_TO_JOIN_HIS_FRIENDLIST, senderInfo.Name);
            await player.SendDialog(acceptPacket, refusePacket, question);
        }
    }
}