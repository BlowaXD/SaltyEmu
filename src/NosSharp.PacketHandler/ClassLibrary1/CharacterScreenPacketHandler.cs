using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ChickenAPI.Accounts;
using ChickenAPI.DAL.Interfaces;
using ChickenAPI.Dtos;
using ChickenAPI.Packets.ClientPackets;
using ChickenAPI.Packets.ServerPackets;
using ChickenAPI.Player;
using ChickenAPI.Player.Enums;
using ChickenAPI.Session;
using ChickenAPI.Utils;
using NosSharp.PacketHandler.Utils;
using CharacterClassType = ChickenAPI.Player.CharacterClassType;

namespace NosSharp.PacketHandler
{
    public class CharacterScreenPacketHandler : ICharacterScreenPacketHandler
    {
        /// <summary>
        /// Char_NEW character creation character
        /// </summary>
        /// <param name="characterCreatePacket"></param>
        /// <param name="session"></param>
        public static void CreateCharacter(CharNewPacket characterCreatePacket, ISession session)
        {
            if (session.HasCurrentMapInstance)
            {
                return;
            }

            // TODO: Hold Account Information in Authorized object
            ulong accountId = session.Account.AccountId;
            byte slot = characterCreatePacket.Slot;
            string characterName = characterCreatePacket.Name;

            var characterService = DependencyContainer.Instance.Get<ICharacterService>();

            if (characterService.GetActiveByAccountId(session.Account.AccountId).FirstOrDefault(s => s.AccountId == accountId && s.Slot == slot && s.State == CharacterState.Active) != null)
            {
                return;
            }
            var rg = new Regex(@"^[\u0021-\u007E\u00A1-\u00AC\u00AE-\u00FF\u4E00-\u9FA5\u0E01-\u0E3A\u0E3F-\u0E5B\u002E]*$");
            if (rg.Matches(characterName).Count == 1)
            {
                CharacterDto character = characterService.GetActiveByAccountId(session.Account.AccountId).FirstOrDefault(s => s.Name == characterName && s.State == CharacterState.Active);
                if (character == null)
                {
                    var rnd = new Random();
                    var newCharacter = new CharacterDto
                    {
                        Class = (byte)CharacterClassType.Adventurer,
                        Gender = characterCreatePacket.Gender,
                        HairColor = characterCreatePacket.HairColor,
                        HairStyle = characterCreatePacket.HairStyle,
                        Hp = 221,
                        JobLevel = 1,
                        Level = 1,
                        MapId = 1,
                        MapX = (short)rnd.Next(78, 81),
                        MapY = (short)rnd.Next(114, 118),
                        Mp = 221,
                        MaxMateCount = 10,
                        SpPoint = 10000,
                        SpAdditionPoint = 0,
                        Name = characterName,
                        Slot = slot,
                        AccountId = accountId,
                        MinilandMessage = "Welcome",
                        State = CharacterState.Active
                    };
                    CharacterDto chara = newCharacter;
                    characterService.Insert(chara);
                    LoadCharacters(null, session);
                }
                else
                {
                    session.SendPacket(new InfoPacket()
                    {
                        Message = "Already_taken"
                    });
                }
            }
            else
            {
                session.SendPacket(new InfoPacket()
                {
                    Message = "invalid_charname"
                });
            }
        }

        /// <summary>
        /// Char_DEL packet
        /// </summary>
        /// <param name="characterDeletePacket"></param>
        public static void DeleteCharacter(CharacterDeletePacket characterDeletePacket, ISession session)
        {
            if (session.HasCurrentMapInstance)
            {
                return;
            }

            AccountDto account = DependencyContainer.Instance.Get<IAccountService>().GetById(session.Account.AccountId);
            var characterService = DependencyContainer.Instance.Get<ICharacterService>();
            if (account == null)
            {
                return;
            }

            if (account.Password.ToLower() != characterDeletePacket.Password)
            {
                session.SendPacket(new InfoPacket()
                {
                    Message = "bad_password"
                });
                return;
            }

            CharacterDto character = characterService.GetActiveByAccountId(session.Account.AccountId)
                .FirstOrDefault(s => s.AccountId == account.AccountId && s.Slot == characterDeletePacket.Slot && s.State == CharacterState.Active);
            if (character == null)
            {
                return;
            }

            character.State = CharacterState.Inactive;
            characterService.Update(character);

            LoadCharacters(null, session);
        }

        /// <summary>
        /// Load Characters, this is the Entrypoint for the Client, Wait for 3 Packets.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public static void LoadCharacters(EntryPointPacket packet, ISession session)
        {
            Console.WriteLine("LoadCharacters");
            if (session.Account == null)
            {
                string name = packet.Name;
                AccountDto accountDto = DependencyContainer.Instance.Get<IAccountService>().GetByName(name);

                if (accountDto == null)
                {
                    //Logger.Log.ErrorFormat(LogLanguage.Instance.GetMessageFromKey(LogLanguageKey.INVALID_ACCOUNT));
                    session.Disconnect();
                    return;
                }

                if (!accountDto.Password.Equals(packet.Password.ToSha512(), StringComparison.OrdinalIgnoreCase))
                {
                    //Logger.Log.ErrorFormat(LogLanguage.Instance.GetMessageFromKey(LogLanguageKey.INVALID_PASSWORD));
                    session.Disconnect();
                    return;
                }

                var accountobject = new AccountDto
                {
                    AccountId = accountDto.AccountId,
                    Name = accountDto.Name,
                    Password = accountDto.Password.ToLower(),
                    Authority = accountDto.Authority
                };
                session.InitializeAccount(accountobject);
                //Send Account Connected
            }

            if (session.Account == null)
            {
                return;
            }

            //Logger.Log.InfoFormat(LogLanguage.Instance.GetMessageFromKey(LogLanguageKey.ACCOUNT_ARRIVED), Session.Account.Name);
            IEnumerable<CharacterDto> characters = DependencyContainer.Instance.Get<ICharacterService>().GetActiveByAccountId(session.Account.AccountId);

            // load characterlist packet for each character in Character
            session.SendPacket(new ClistStartPacket { Type = 0 });
            foreach (CharacterDto character in characters)
            {

                //WearableInstance[] equipment = new WearableInstance[16];
                /* IEnumerable<ItemInstanceDTO> inventory = DAOFactory.IteminstanceDAO.Where(s => s.CharacterId == character.CharacterId && s.Type == (byte)InventoryType.Wear);
                 foreach (ItemInstanceDTO equipmentEntry in inventory)
                 {
                     // explicit load of iteminstance
                     WearableInstance currentInstance = equipmentEntry as WearableInstance;
                     equipment[(short)currentInstance.Item.EquipmentSlot] = currentInstance;
                 }
                    */
                List<short?> petlist = new List<short?>();
                IList<MateDto> mates = DependencyContainer.Instance.Get<IMateService>().GetMatesByCharacterId(character.CharacterId);
                for (int i = 0; i < 26; i++)
                {
                    if (mates.Count > i)
                    {
                        petlist.Add(mates[i].Skin);
                        petlist.Add(mates[i].VNum);
                    }
                    else
                    {
                        petlist.Add(-1);
                    }
                }

                session.SendPacket(new ClistPacket
                {
                    Slot = character.Slot,
                    Name = character.Name,
                    Unknown = 0,
                    Gender = (byte)character.Gender,
                    HairStyle = (byte)character.HairStyle,
                    HairColor = (byte)character.HairColor,
                    Unknown1 = 0,
                    Class = (CharacterClassType)character.Class,
                    Level = character.Level,
                    HeroLevel = character.HeroLevel,
                    Equipments = new List<short?>()
                    {
                        4986
                        /*
                        {equipment[(byte)EquipmentType.Hat]?.VNum ?? -1},
                        {equipment[(byte)EquipmentType.Armor]?.VNum ?? -1},
                        {equipment[(byte)EquipmentType.WeaponSkin]?.VNum ?? (equipment[(byte)EquipmentType.MainWeapon]?.VNum ?? -1)},
                        {equipment[(byte)EquipmentType.SecondaryWeapon]?.VNum ?? -1},
                        { equipment[(byte)EquipmentType.Mask]?.VNum ?? -1 },
                        { equipment[(byte)EquipmentType.Fairy]?.VNum ?? -1 },
                        { equipment[(byte)EquipmentType.CostumeSuit]?.VNum ?? -1},
                        { equipment[(byte)EquipmentType.CostumeHat]?.VNum ?? -1}
                        */
                    },
                    JobLevel = character.JobLevel,
                    QuestCompletion = 1,
                    QuestPart = 1,
                    Pets = petlist,
                    Design = 1, //(equipment[(byte)EquipmentType.Hat]?.Item.IsColored == true ? equipment[(byte)EquipmentType.Hat].Design : 0),
                    Unknown3 = 0
                });
                // 1 1 before long string of -1.-1 = act completion

            }
            session.SendPacket(new ClistEndPacket());
        }

    }
}
