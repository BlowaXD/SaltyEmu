using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Enums.Game.Character;
using ChickenAPI.Game._i18n;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets.ClientPackets.CharacterSelectionScreen;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.UI;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.CharacterScreen
{
    public class CreateCharacterHandler : GenericSessionPacketHandlerAsync<CharNewPacket>
    {
        private readonly ICharacterService _characterService;
        private readonly IGameLanguageService _gameLanguageService;
        private readonly CharacterScreenLoadHandler _screenLoader;


        public CreateCharacterHandler(ILogger log, ICharacterService characterService, IGameLanguageService gameLanguageService, CharacterScreenLoadHandler screenLoader) : base(log)
        {
            _characterService = characterService;
            _gameLanguageService = gameLanguageService;
            _screenLoader = screenLoader;
        }

        protected override async Task Handle(CharNewPacket packet, ISession session)
        {
            long accountId = session.Account.Id;
            byte slot = packet.Slot;
            string characterName = packet.Name;

            if (slot > 3)
            {
                return;
            }

            if (await _characterService.GetByAccountIdAndSlotAsync(session.Account.Id, slot) != null)
            {
                Log.Warn($"[CREATE_CHARACTER] SLOT_ALREADY_TAKEN {slot}");
                return;
            }

            var rg = new Regex(@"^[\u0021-\u007E\u00A1-\u00AC\u00AE-\u00FF\u4E00-\u9FA5\u0E01-\u0E3A\u0E3F-\u0E5B\u002E]*$");
            if (rg.Matches(characterName).Count != 1)
            {
                await session.SendPacketAsync(new InfoPacket
                {
                    Message = _gameLanguageService.GetLanguage(PlayerMessages.CHARACTER_NAME_INVALID, session.Language)
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_NAME {characterName}");
                return;
            }

            CharacterDto character = await _characterService.GetActiveByNameAsync(characterName);
            if (character != null)
            {
                await session.SendPacketAsync(new InfoPacket
                {
                    Message = _gameLanguageService.GetLanguage(PlayerMessages.CHARACTER_NAME_ALREADY_TAKEN, session.Language)
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_NAME {characterName}");
                return;
            }

            CharacterDto newCharacter = _characterService.GetCreationCharacter();

            newCharacter.Class = CharacterClassType.Adventurer;
            newCharacter.Gender = packet.Gender;
            newCharacter.HairColor = packet.HairColor;
            newCharacter.HairStyle = packet.HairStyle;
            newCharacter.Name = characterName;
            newCharacter.Slot = slot;
            newCharacter.AccountId = accountId;
            newCharacter.MinilandMessage = "Welcome";
            newCharacter.State = CharacterState.Active;
            await _characterService.SaveAsync(newCharacter);
            Log.Info($"[CHARACTER_CREATE] {newCharacter.Name} | Account : {session.Account.Name}");
            await _screenLoader.Handle(session);
        }
    }
}