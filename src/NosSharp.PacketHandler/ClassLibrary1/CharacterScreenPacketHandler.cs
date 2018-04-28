using System;
using System.Collections.Generic;
using ChickenAPI.Accounts;
using ChickenAPI.DAL.Interfaces;
using ChickenAPI.Dtos;
using ChickenAPI.Packets;
using ChickenAPI.Packets.ClientPackets;
using ChickenAPI.Packets.ServerPackets;
using ChickenAPI.Player;
using ChickenAPI.Session;
using ChickenAPI.Utils;
using NosSharp.PacketHandler.Utils;

namespace NosSharp.PacketHandler
{
    public class CharacterScreenPacketHandler : ICharacterScreenPacketHandler
    {

        private ClistPacket GenerateCListPacket(CharacterDto character)
        {
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
            
            return new ClistPacket
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
            };
        }

        /// <summary>
        /// Load Characters, this is the Entrypoint for the Client, Wait for 3 Packets.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public void LoadCharacters(EntryPointPacket packet, ISession session)
        {
            if (session.AccountDto == null)
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
                    Id = accountDto.Id,
                    Name = accountDto.Name,
                    Password = accountDto.Password.ToLower(),
                    Authority = accountDto.Authority
                };
                session.InitializeAccount(accountobject);
                //Send AccountDto Connected
            }

            if (session.AccountDto == null)
            {
                return;
            }

            //Logger.Log.InfoFormat(LogLanguage.Instance.GetMessageFromKey(LogLanguageKey.ACCOUNT_ARRIVED), Session.AccountDto.Name);
            IEnumerable<CharacterDto> characters = DependencyContainer.Instance.Get<ICharacterService>().GetActiveByAccountId(session.AccountDto.Id);

            // load characterlist packet for each character in Character
            session.SendPacket(new ClistStartPacket { Type = 0 });
            foreach (CharacterDto character in characters)
            {
                session.SendPacket(GenerateCListPacket(character));
                //WearableInstance[] equipment = new WearableInstance[16];
                /* IEnumerable<ItemInstanceDTO> inventory = DAOFactory.IteminstanceDAO.Where(s => s.CharacterId == character.CharacterId && s.Type == (byte)InventoryType.Wear);
                 foreach (ItemInstanceDTO equipmentEntry in inventory)
                 {
                     // explicit load of iteminstance
                     WearableInstance currentInstance = equipmentEntry as WearableInstance;
                     equipment[(short)currentInstance.Item.EquipmentSlot] = currentInstance;
                 }
                    */

                // 1 1 before long string of -1.-1 = act completion
            }
            session.SendPacket(new ClistEndPacket());
        }

    }
}
