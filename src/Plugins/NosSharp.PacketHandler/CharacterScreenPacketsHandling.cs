using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Utils;
using ChickenAPI.Enums.Game;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Data.AccessLayer.Account;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Data.AccessLayer.Server;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Data.TransferObjects.Item;
using ChickenAPI.Game.Data.TransferObjects.Server;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Packets.CharacterScreen.Client;
using ChickenAPI.Game.Packets.CharacterScreen.Server;

namespace NosSharp.PacketHandler
{
    public class CharacterScreenPacketsHandling
    {
        private static ILanguageService _languageService;

        private static ILanguageService Language
        {
            get => _languageService ?? (_languageService = ChickenContainer.Instance.Resolve<ILanguageService>());
        }

        private static readonly Logger Log = Logger.GetLogger<CharacterScreenPacketsHandling>();

        /// <summary>
        /// Char_DEL packetBase
        /// </summary>
        /// <param name="characterDeletePacketBase"></param>
        /// <param name="session"></param>
        public static async void DeleteCharacter(CharacterDeletePacketBase characterDeletePacketBase, ISession session)
        {
            AccountDto account = await ChickenContainer.Instance.Resolve<IAccountService>().GetByIdAsync(session.Account.Id);
            var characterService = ChickenContainer.Instance.Resolve<ICharacterService>();
            if (account == null)
            {
                return;
            }

            if (!string.Equals(account.Password, characterDeletePacketBase.Password.ToSha512(), StringComparison.CurrentCultureIgnoreCase))
            {
                session.SendPacket(new InfoPacketBase
                {
                    Message = "bad_password"
                });
                return;
            }

            CharacterDto character = await characterService.GetByAccountIdAndSlotAsync(session.Account.Id, characterDeletePacketBase.Slot);
            if (character == null)
            {
                return;
            }

            character.State = CharacterState.Inactive;
            await characterService.DeleteByIdAsync(character.Id);
            OnLoadCharacters(null, session);
        }


        /// <summary>
        /// Char_NEW characterEntity creation characterEntity
        /// </summary>
        /// <param name="characterCreatePacketBase"></param>
        /// <param name="session"></param>
        public static async void OnCreateCharacter(CharNewPacketBase characterCreatePacketBase, ISession session)
        {
            long accountId = session.Account.Id;
            byte slot = characterCreatePacketBase.Slot;
            string characterName = characterCreatePacketBase.Name;

            var characterService = ChickenContainer.Instance.Resolve<ICharacterService>();

            if (slot > 3)
            {
                return;
            }

            if (await characterService.GetByAccountIdAndSlotAsync(session.Account.Id, slot) != null)
            {
                Log.Warn($"[CREATE_CHARACTER] SLOT_ALREADY_TAKEN {slot}");
                return;
            }

            var rg = new Regex(@"^[\u0021-\u007E\u00A1-\u00AC\u00AE-\u00FF\u4E00-\u9FA5\u0E01-\u0E3A\u0E3F-\u0E5B\u002E]*$");
            if (rg.Matches(characterName).Count != 1)
            {
                session.SendPacket(new InfoPacketBase
                {
                    Message = Language.GetLanguage(ChickenI18NKey.CHARACTER_NAME_INVALID, session.Langage)
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_NAME {characterName}");
                return;
            }

            CharacterDto character = await characterService.GetActiveByNameAsync(characterName);
            if (character != null)
            {
                session.SendPacket(new InfoPacketBase
                {
                    Message = Language.GetLanguage(ChickenI18NKey.CHARACTER_NAME_ALREADY_TAKEN, session.Langage)
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_NAME {characterName}");
                return;
            }

            CharacterDto newCharacter = characterService.GetCreationCharacter();

            newCharacter.Class = CharacterClassType.Adventurer;
            newCharacter.Gender = characterCreatePacketBase.Gender;
            newCharacter.HairColor = characterCreatePacketBase.HairColor;
            newCharacter.HairStyle = characterCreatePacketBase.HairStyle;
            newCharacter.Name = characterName;
            newCharacter.Slot = slot;
            newCharacter.AccountId = accountId;
            newCharacter.MinilandMessage = "Welcome";
            newCharacter.State = CharacterState.Active;
            await characterService.SaveAsync(newCharacter);
            Log.Info($"[CHARACTER_CREATE] {newCharacter.Name} | Account : {session.Account.Name}");
            OnLoadCharacters(null, session);
        }

        /// <summary>
        /// Char_NEW characterEntity creation characterEntity
        /// </summary>
        /// <param name="characterCreatePacketBase"></param>
        /// <param name="session"></param>
        public static async void OnCreateCharacterWrestler(CharNewWrestlerPacketBase characterCreatePacketBase, ISession session)
        {
            long accountId = session.Account.Id;
            byte slot = characterCreatePacketBase.Slot;
            string characterName = characterCreatePacketBase.Name;

            var characterService = ChickenContainer.Instance.Resolve<ICharacterService>();

            if (slot > 3)
            {
                return;
            }

            if (slot != 3)
            {
                session.SendPacket(new InfoPacketBase
                {
                    Message = "invalid_slot_wrestler"
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_SLOT_WRESTLER {slot}");
                return;
            }

            if (!characterService.GetActiveByAccountId(session.Account.Id).Any(s => s.Level >= 80))
            {
                session.SendPacket(new InfoPacketBase
                {
                    Message = "invalid_lvl_wrestler"
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_LVL_WRESTLER");
                return;
            }

            if (await characterService.GetByAccountIdAndSlotAsync(session.Account.Id, slot) != null)
            {
                Log.Warn($"[CREATE_CHARACTER] SLOT_ALREADY_TAKEN {slot}");
                return;
            }

            var rg = new Regex(@"^[\u0021-\u007E\u00A1-\u00AC\u00AE-\u00FF\u4E00-\u9FA5\u0E01-\u0E3A\u0E3F-\u0E5B\u002E]*$");
            if (rg.Matches(characterName).Count != 1)
            {
                session.SendPacket(new InfoPacketBase
                {
                    Message = "invalid_charname"
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_NAME {characterName}");
                return;
            }

            CharacterDto character = await characterService.GetActiveByNameAsync(characterName);
            if (character != null)
            {
                session.SendPacket(new InfoPacketBase
                {
                    Message = "Already_taken"
                });
                Log.Warn($"[CREATE_CHARACTER] INVALID_NAME {characterName}");
                return;
            }

            CharacterDto newCharacter = characterService.GetCreationCharacter();

            newCharacter.Class = CharacterClassType.Wrestler;
            newCharacter.Gender = characterCreatePacketBase.Gender;
            newCharacter.HairColor = characterCreatePacketBase.HairColor;
            newCharacter.HairStyle = characterCreatePacketBase.HairStyle;
            newCharacter.Name = characterName;
            newCharacter.Slot = slot;
            newCharacter.AccountId = accountId;
            newCharacter.MinilandMessage = "Welcome";
            newCharacter.State = CharacterState.Active;
            await characterService.SaveAsync(newCharacter);
            Log.Info($"[CHARACTER_CREATE] {newCharacter.Name} | Account : {accountId}");
            OnLoadCharacters(null, session);
        }

        private static async Task<bool> AccountChecks(EntryPointPacketBase packetBase, ISession session)
        {
            string name = packetBase.Name;
            AccountDto accountDto = await ChickenContainer.Instance.Resolve<IAccountService>().GetByNameAsync(name);

            if (accountDto == null)
            {
                Log.Warn($"Invalid account {name}");
                session.Disconnect();
                return false;
            }

            if (!string.Equals(packetBase.Password.ToSha512(), accountDto.Password, StringComparison.OrdinalIgnoreCase))
            {
                Log.Warn($"Invalid account password {name}");
                //Logger.Log.ErrorFormat("Invalid Password");
                session.Disconnect();
                return false;
            }

            PlayerSessionDto sessionDto = ChickenContainer.Instance.Resolve<ISessionService>().GetByAccountName(name);
            if (sessionDto == null || sessionDto.Id != session.SessionId)
            {
                Log.Warn($"No session, hijacking tried by {session.Ip}");
                session.Disconnect();
                return false;
            }

            if (!string.Equals(sessionDto.Username, accountDto.Name, StringComparison.CurrentCultureIgnoreCase) ||
                !string.Equals(sessionDto.Password, accountDto.Password, StringComparison.CurrentCultureIgnoreCase))
            {
                Log.Warn($"Info mismatch with session {sessionDto.Id}");
                session.Disconnect();
                return false;
            }

            if (sessionDto.State == PlayerSessionState.Connected)
            {
                Log.Warn($"{sessionDto.State} Already connected !");
                session.Disconnect();
                return false;
            }

            var accountobject = new AccountDto
            {
                Id = accountDto.Id,
                Name = accountDto.Name,
                Password = accountDto.Password.ToLower(),
                Authority = accountDto.Authority
            };
            session.InitializeAccount(accountobject);
            return true;
        }

        /// <summary>
        /// Load Characters, this is the Entrypoint for the Client, Wait for 3 Packets.
        /// </summary>
        /// <param name="packetBase"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static async void OnLoadCharacters(EntryPointPacketBase packetBase, ISession session)
        {
            if (session.Account == null && !await AccountChecks(packetBase, session))
            {
                return;
            }

            Log.Info($"[LOAD_CHARACTERS] {session.Account.Name}");
            IEnumerable<CharacterDto> characters = await ChickenContainer.Instance.Resolve<ICharacterService>().GetActiveByAccountIdAsync(session.Account.Id);

            // load characterlist packetBase for each characterEntity in Player
            session.SendPacket(new ClistStartPacketBase { Type = 0 });
            foreach (CharacterDto character in characters)
            {
                ItemInstanceDto[] equipment = new ItemInstanceDto[16];
                IEnumerable<ItemInstanceDto> inventory = await ChickenContainer.Instance.Resolve<IItemInstanceService>().GetWearByCharacterIdAsync(character.Id);
                foreach (ItemInstanceDto equipmentEntry in inventory)
                {
                    equipment[(short)equipmentEntry.Item.EquipmentSlot] = equipmentEntry;
                }

                List<short?> petlist = new List<short?>();
                CharacterMateDto[] mates = await ChickenContainer.Instance.Resolve<ICharacterMateService>().GetMatesByCharacterIdAsync(character.Id);
                for (int i = 0; i < 26; i++)
                {
                    if (mates.Length > i)
                    {
                        petlist.Add(mates[i]?.Skin ?? -1);
                        petlist.Add(mates[i]?.VNum ?? -1);
                    }
                    else
                    {
                        petlist.Add(-1);
                    }
                }

                session.SendPacket(new ClistPacketBase
                {
                    Slot = character.Slot,
                    Name = character.Name,
                    Unknown = 0,
                    Gender = character.Gender,
                    HairStyle = character.HairStyle,
                    HairColor = character.HairColor,
                    Unknown1 = 0,
                    Class = character.Class,
                    Level = character.Level,
                    HeroLevel = character.HeroLevel,
                    Equipments = new List<short?>
                    {
                        (short?)(equipment[(byte)EquipmentType.Hat]?.ItemId ?? -1),
                        (short?)(equipment[(byte)EquipmentType.Armor]?.ItemId ?? -1),
                        (short?)(equipment[(byte)EquipmentType.WeaponSkin]?.ItemId ?? (equipment[(byte)EquipmentType.MainWeapon]?.ItemId ?? -1)),
                        (short?)(equipment[(byte)EquipmentType.SecondaryWeapon]?.ItemId ?? -1),
                        (short?)(equipment[(byte)EquipmentType.Mask]?.ItemId ?? -1),
                        (short?)(equipment[(byte)EquipmentType.Fairy]?.ItemId ?? -1),
                        (short?)(equipment[(byte)EquipmentType.CostumeSuit]?.ItemId ?? -1),
                        (short?)(equipment[(byte)EquipmentType.CostumeHat]?.ItemId ?? -1)
                    },
                    JobLevel = character.JobLevel,
                    QuestCompletion = 1,
                    QuestPart = 1,
                    Pets = petlist,
                    Design = equipment[(byte)EquipmentType.Hat]?.Item.IsColored == true ? equipment[(byte)EquipmentType.Hat].Design : 0,
                    Unknown3 = 0
                });
            }

            session.SendPacket(new ClistEndPacketBase());
        }

        public static async void OnSelectCharacter(SelectPacketBase packetBase, ISession session)
        {
            try
            {
                if (session?.Account == null)
                {
                    return;
                }

                CharacterDto characterDto = await ChickenContainer.Instance.Resolve<ICharacterService>().GetByAccountIdAndSlotAsync(session.Account.Id, packetBase.Slot);
                if (characterDto == null)
                {
                    return;
                }

                session.InitializeCharacterId(characterDto.Id);
                session.SendPacket(new OkPacketBase());
            }
            catch (Exception ex)
            {
                Log.Error("Select characterEntity failed.", ex);
            }
        }
    }
}