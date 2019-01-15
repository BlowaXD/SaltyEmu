using System;
using System.Threading.Tasks;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets.CharacterSelectionScreen.Client;
using ChickenAPI.Packets.Game.Server.UserInterface;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.CharacterScreen
{
    public class DeleteCharacterHandler : GenericSessionPacketHandlerAsync<CharacterDeletePacketBase>
    {
        private readonly IAccountService _accountService;
        private readonly ICharacterService _characterService;
        private readonly CharacterScreenLoadHandler _screenLoader;

        public DeleteCharacterHandler(IAccountService accountService, ICharacterService characterService, CharacterScreenLoadHandler screenLoader)
        {
            _accountService = accountService;
            _characterService = characterService;
            _screenLoader = screenLoader;
        }

        protected override async Task Handle(CharacterDeletePacketBase packet, ISession session)
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