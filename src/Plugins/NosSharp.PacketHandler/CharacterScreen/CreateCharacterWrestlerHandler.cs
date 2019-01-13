using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets.CharacterSelectionScreen.Client;
using ChickenAPI.Packets.Game.Server.UserInterface;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.CharacterScreen
{
    public class CreateCharacterWrestlerHandler : GenericSessionPacketHandlerAsync<CharNewWrestlerPacketBase>
    {
        private readonly ICharacterService _characterService;
        private readonly CharacterScreenLoadHandler _screenLoader;

        public CreateCharacterWrestlerHandler(ICharacterService characterService, CharacterScreenLoadHandler screenLoader)
        {
            _characterService = characterService;
            _screenLoader = screenLoader;
        }

        protected override async Task Handle(CharNewWrestlerPacketBase packet, ISession session)
        {
            long accountId = session.Account.Id;
            byte slot = packet.Slot;
            string characterName = packet.Name;

            if (slot > 3)
            {
                return;
            }

            if (slot != 3)
            {
                await session.SendPacketAsync(new InfoPacket
                {
                    Message = "invalid_slot_wrestler"
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_SLOT_WRESTLER {slot}");
                return;
            }

            if (!_characterService.GetActiveByAccountId(session.Account.Id).Any(s => s.Level >= 80))
            {
                await session.SendPacketAsync(new InfoPacket
                {
                    Message = "invalid_lvl_wrestler"
                });
                Log.Warn("[CREATE_CHARACTER] INVALID_LVL_WRESTLER");
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
                    Message = "invalid_charname"
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_NAME {characterName}");
                return;
            }

            CharacterDto character = await _characterService.GetActiveByNameAsync(characterName);
            if (character != null)
            {
                await session.SendPacketAsync(new InfoPacket
                {
                    Message = "Already_taken"
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_NAME {characterName}");
                return;
            }

            CharacterDto newCharacter = _characterService.GetCreationCharacter();

            newCharacter.Class = CharacterClassType.Wrestler;
            newCharacter.Gender = packet.Gender;
            newCharacter.HairColor = packet.HairColor;
            newCharacter.HairStyle = packet.HairStyle;
            newCharacter.Name = characterName;
            newCharacter.Slot = slot;
            newCharacter.AccountId = accountId;
            newCharacter.MinilandMessage = "Welcome";
            newCharacter.State = CharacterState.Active;
            await _characterService.SaveAsync(newCharacter);
            Log.Info($"[CHARACTER_CREATE] {newCharacter.Name} | Account : {accountId}");
            await _screenLoader.Handle(session);
        }
    }
}