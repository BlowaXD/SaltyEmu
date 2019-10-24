using System;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Enums.Game.Character;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets.ClientPackets.CharacterSelectionScreen;
using ChickenAPI.Packets.ServerPackets.UI;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.CharacterScreen
{
    public class DeleteCharacterHandler : GenericSessionPacketHandlerAsync<CharacterDeletePacket>
    {
        private readonly IAccountService _accountService;
        private readonly ICharacterService _characterService;
        private readonly CharacterScreenLoadHandler _screenLoader;

        public DeleteCharacterHandler(ILogger log, IAccountService accountService, ICharacterService characterService, CharacterScreenLoadHandler screenLoader) : base(log)
        {
            _accountService = accountService;
            _characterService = characterService;
            _screenLoader = screenLoader;
        }

        protected override async Task Handle(CharacterDeletePacket packet, ISession session)
        {
            AccountDto account = await _accountService.GetByIdAsync(session.Account.Id);
            if (account == null)
            {
                return;
            }

            if (!string.Equals(account.Password, packet.Password.ToSha512(), StringComparison.CurrentCultureIgnoreCase))
            {
                await session.SendPacketAsync(new InfoPacket
                {
                    Message = "bad_password"
                });
                return;
            }

            CharacterDto character = await _characterService.GetByAccountIdAndSlotAsync(session.Account.Id, packet.Slot);
            if (character == null)
            {
                return;
            }

            character.State = CharacterState.Inactive;
            await _characterService.DeleteByIdAsync(character.Id);
            await _screenLoader.Handle(session);
        }
    }
}