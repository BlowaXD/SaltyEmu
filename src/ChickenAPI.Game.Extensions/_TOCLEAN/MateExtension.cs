using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets.Game.Server.Mates;
using ChickenAPI.Packets.Game.Server.UserInterface;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class MateExtension
    {
        private static readonly ICharacterMateService CharacterMateService = new Lazy<ICharacterMateService>(() => ChickenContainer.Instance.Resolve<ICharacterMateService>()).Value;

        public static async Task<bool> AddPet(this IPlayerEntity player, IMateEntity mate)
        {
            if (mate.Mate.MateType == MateType.Pet
                ? player.Character.MaxMateCount <= player.Mates.Count
                : 3 <= player.Mates.Count(s => s.Mate.MateType == MateType.Partner))
            {
                return false;
            }

            CharacterMateService.Save(mate.Mate);
            await player.BroadcastAsync(mate.GenerateInPacket());
            await player.SendPacketAsync(new PClearPacket());
            player.Mates.Add(mate);
            await player.SendPacketsAsync(player.GenerateScP());
            await player.SendPacketsAsync(player.GenerateScN());
            await player.SendChatMessageAsync(mate.Mate.MateType == MateType.Pet ? PlayerMessages.PETS_YOU_GET_X_AS_A_NEW_PET : PlayerMessages.PETS_YOU_GET_X_AS_A_NEW_PARTNER, SayColorType.Green);
            /*mate.RefreshStats();*/
            return true;
        }

        public static List<ScnPacket> GenerateScN(this IPlayerEntity player)
        {
            List<ScnPacket> list = new List<ScnPacket>();
            byte i = 0;
            foreach (IMateEntity y in player.GetMateByMateType(MateType.Partner))
            {
                y.PetId = i;
                //y.LoadInventory();
                list.Add(y.GenerateScnPacket());
                i++;
            }

            return list;
        }

        public static List<ScpPacket> GenerateScP(this IPlayerEntity player, byte page = 0)
        {
            List<ScpPacket> list = new List<ScpPacket>();
            byte i = 0;
            foreach (IMateEntity y in player.GetMateByPage(MateType.Pet, page))
            {
                y.PetId = i;
                list.Add(y.GenerateScpPacket());
                i++;
            }

            return list;
        }

        public static ScnPacket GenerateScnPacket(this IMateEntity mate) =>
            new ScnPacket
            {
                PetId = mate.PetId,
                NpcMonsterVNum = mate.Mate.NpcMonsterId,
                TransportId = mate.Id,
                Level = mate.Level,
                Loyalty = mate.Mate.Loyalty,
                Experience = mate.Mate.Experience,
                WeaponInstanceDetails = new ScnPacket.ScEquipmentDetails
                {
                    ItemId = -1 // (WeaponInstance != null ? $"{WeaponInstance.ItemVNum}.{WeaponInstance.Rare}.{WeaponInstance.Upgrade}" : "-1")
                },
                ArmorInstanceDetails = new ScnPacket.ScEquipmentDetails
                {
                    ItemId = -1 // {(ArmorInstance != null ? $"{ArmorInstance.ItemVNum}.{ArmorInstance.Rare}.{ArmorInstance.Upgrade}" : "-1")}
                },
                GauntletInstanceDetails = new ScnPacket.ScEquipmentDetails
                {
                    ItemId = -1 // {(GauntletInstanceDetails != null ? $"{ArmorInstance.ItemVNum}.{ArmorInstance.Rare}.{ArmorInstance.Upgrade}" : "-1")}
                },
                BootsInstanceDetails = new ScnPacket.ScEquipmentDetails
                {
                    ItemId = -1 // {(BootsInstanceDetails != null ? $"{ArmorInstance.ItemVNum}.{ArmorInstance.Rare}.{ArmorInstance.Upgrade}" : "-1")}
                },
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 1,
                AttackUpgrade = mate.Mate.Attack,
                MinimumAttack = mate.MinHit,
                MaximumAttack = mate.MaxHit,
                Precision = mate.HitRate,
                CriticalRate = mate.CriticalRate,
                CriticalDamageRate = mate.CriticalChance,
                Defence = mate.Defence,
                DefenceDodge = mate.DefenceDodge,
                DistanceDefence = mate.DistanceDefence,
                DistanceDodge = mate.DistanceDefenceDodge,
                DodgeRate = mate.DefenceDodge,
                ElementRate = 1519,
                FireResistance = mate.FireResistance,
                WaterResistance = mate.WaterResistance,
                LightResistance = mate.LightResistance,
                DarkResistance = mate.DarkResistance,
                Hp = mate.Hp,
                HpMax = mate.HpMax,
                Mp = mate.Mp,
                MpMax = mate.MpMax,
                Unknown21 = 0,
                Unknown22 = 285816,
                Name = mate.Mate.Name.Replace(' ', '^'),
                MorphId = -1, //(IsUsingSp && SpInstance != null ? SpInstance.Item.Morph : Skin != 0 ? Skin : -1)
                IsSummonable = mate.Mate.IsSummonable,
                SpDetails = new ScnPacket.ScSpDetails
                {
                    ItemId = -1 //(SpInstance != null ? $"{SpInstance.ItemVNum}.100" : "-1")
                },
                Skill1Details = new ScnPacket.ScSkillDetails
                {
                    SkillId = -1 //
                },
                Skill2Details = new ScnPacket.ScSkillDetails
                {
                    SkillId = -1 //
                },
                Skill3Details = new ScnPacket.ScSkillDetails
                {
                    SkillId = -1 //
                }
            };

        public static ScpPacket GenerateScpPacket(this IMateEntity mate) =>
            new ScpPacket
            {
                PetId = mate.PetId,
                AttackUpgrade = mate.Mate.Attack,
                CanPickUp = mate.Mate.CanPickUp,
                CloseDefence = mate.Defence,
                Concentrate = mate.HitRate,
                CriticalChance = mate.CriticalChance,
                CriticalRate = mate.CriticalRate,
                DamageMaximum = mate.MinHit,
                DamageMinimum = mate.MaxHit,
                DarkResistance = mate.DarkResistance,
                DefenceDodge = mate.DefenceDodge,
                DefenceUpgrade = mate.Mate.Defence,
                DistanceDefence = mate.DistanceDefence,
                DistanceDefenceDodge = mate.DistanceDefenceDodge,
                Element = 0,
                Experience = mate.Mate.Experience,
                FireResistance = mate.FireResistance,
                Hp = mate.Hp,
                IsSummonable = mate.Mate.IsSummonable,
                IsTeamMember = mate.Mate.IsTeamMember,
                Level = mate.Level,
                LightResistance = mate.LightResistance,
                Loyalty = mate.Mate.Loyalty,
                MagicDefence = mate.MagicalDefence,
                MaxHp = mate.HpMax,
                MaxMp = mate.MpMax,
                Mp = mate.Mp,
                Name = mate.Mate.Name,
                NpcMonsterVNum = mate.Mate.NpcMonsterId,
                WaterResistance = mate.WaterResistance,
                TransportId = mate.Mate.Id,
                Unknow1 = 0,
                XpLoad = 10
            };
    }
}