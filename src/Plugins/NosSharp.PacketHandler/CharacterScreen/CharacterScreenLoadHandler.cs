using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Server;
using ChickenAPI.Enums.Game;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets.CharacterSelectionScreen.Client;
using ChickenAPI.Packets.CharacterSelectionScreen.Server;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.CharacterScreen
{
    public class CharacterScreenLoadHandler : GenericSessionPacketHandlerAsync<EntryPointPacketBase>
    {
        private readonly IAccountService _accountService;
        private readonly ICharacterMateService _characterMateService;
        private readonly ICharacterService _characterService;
        private readonly IItemInstanceService _itemInstanceService;
        private readonly ISessionService _sessionService;

        public CharacterScreenLoadHandler(IAccountService accountService, ISessionService sessionService, ICharacterService characterService, IItemInstanceService itemInstanceService,
            ICharacterMateService characterMateService)
        {
            _accountService = accountService;
            _sessionService = sessionService;
            _characterService = characterService;
            _itemInstanceService = itemInstanceService;
            _characterMateService = characterMateService;
        }

        private async Task<bool> AccountChecks(EntryPointPacketBase packetBase, ISession session)
        {
            string name = packetBase.Name;
            AccountDto accountDto = await _accountService.GetByNameAsync(name);

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

            PlayerSessionDto sessionDto = _sessionService.GetByAccountName(name);
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

        public Task Handle(ISession session) => Handle(null, session);

        protected override async Task Handle(EntryPointPacketBase packet, ISession session)
        {
            if (session.Account == null && !await AccountChecks(packet, session))
            {
                return;
            }

            Log.Info($"[LOAD_CHARACTERS] {session.Account.Name}");
            IEnumerable<CharacterDto> characters = await _characterService.GetActiveByAccountIdAsync(session.Account.Id);

            // load characterlist packetBase for each characterEntity in Player
            await session.SendPacketAsync(new CListStartPacket { Type = 0 });
            foreach (CharacterDto character in characters)
            {
                ItemInstanceDto[] equipment = new ItemInstanceDto[16];
                IEnumerable<ItemInstanceDto> inventory = await _itemInstanceService.GetWearByCharacterIdAsync(character.Id);
                foreach (ItemInstanceDto equipmentEntry in inventory)
                {
                    equipment[(short)equipmentEntry.Item.EquipmentSlot] = equipmentEntry;
                }

                List<short?> petlist = new List<short?>();
                CharacterMateDto[] mates = await _characterMateService.GetMatesByCharacterIdAsync(character.Id);
                for (int i = 0; i < 26; i++)
                {
                    if (mates.Length > i)
                    {
                        petlist.Add(mates[i]?.Skin ?? -1);
                        petlist.Add(mates[i]?.NpcMonsterId ?? -1);
                    }
                    else
                    {
                        petlist.Add(-1);
                    }
                }

                await session.SendPacketAsync(new ClistPacketBase
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
                        (short?)(equipment[(byte)EquipmentType.Hat]?.Item.Vnum ?? -1),
                        (short?)(equipment[(byte)EquipmentType.Armor]?.Item.Vnum ?? -1),
                        (short?)(equipment[(byte)EquipmentType.WeaponSkin]?.ItemId ?? (equipment[(byte)EquipmentType.MainWeapon]?.Item.Vnum ?? -1)),
                        (short?)(equipment[(byte)EquipmentType.SecondaryWeapon]?.Item.Vnum ?? -1),
                        (short?)(equipment[(byte)EquipmentType.Mask]?.Item.Vnum ?? -1),
                        (short?)(equipment[(byte)EquipmentType.Fairy]?.Item.Vnum ?? -1),
                        (short?)(equipment[(byte)EquipmentType.CostumeSuit]?.Item.Vnum ?? -1),
                        (short?)(equipment[(byte)EquipmentType.CostumeHat]?.Item.Vnum ?? -1)
                    },
                    JobLevel = character.JobLevel,
                    QuestCompletion = 1,
                    QuestPart = 1,
                    Pets = petlist,
                    Design = equipment[(byte)EquipmentType.Hat]?.Item.IsColored == true ? equipment[(byte)EquipmentType.Hat].Design : 0,
                    Unknown3 = 0
                });
            }

            await session.SendPacketAsync(new ClistEndPacketBase());
        }
    }
}