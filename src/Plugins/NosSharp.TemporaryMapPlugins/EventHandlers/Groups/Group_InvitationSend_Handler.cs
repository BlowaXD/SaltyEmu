using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.i18n;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Groups.Events;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Server.Group;
using ChickenAPI.Packets.Game.Server.UserInterface;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Group_InvitationSend_Handler : GenericEventPostProcessorBase<GroupInvitationSendEvent>
    {
        private readonly IPlayerManager _playerManager;
        private readonly ICharacterService _characterService;

        public Group_InvitationSend_Handler(IPlayerManager playerManager, ICharacterService characterService)
        {
            _playerManager = playerManager;
            _characterService = characterService;
        }

        protected override async Task Handle(GroupInvitationSendEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            CharacterDto senderInfo;
            IPlayerEntity target = _playerManager.GetPlayerByCharacterId(e.Target.Character.Id);
            if (target == null)
            {
                senderInfo = await _characterService.GetByIdAsync(e.Sender.Id);
                if (senderInfo == null)
                {
                    return;
                }
            }
            else
            {
                senderInfo = target.Character;
            }

            string playerMessage = target.GetLanguageFormat(PlayerMessages.PLAYER_X_INVITED_TO_YOUR_GROUP, senderInfo.Name);
            player.SendPacketAsync(new InfoPacket
            {
                Message = playerMessage
            });
            PacketBase acceptPacket = new PJoinPacket { CharacterId = e.Target.Character.Id, RequestType = PJoinPacketType.Accepted };
            PacketBase refusePacket = new PJoinPacket { CharacterId = e.Target.Character.Id, RequestType = PJoinPacketType.Declined };
            string question = target.GetLanguageFormat(PlayerMessages.PLAYER_X_INVITED_YOU_TO_JOIN_HIS_GROUP, senderInfo.Name);
            await target.SendDialog(acceptPacket, refusePacket, question);
        }
    }
}