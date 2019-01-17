using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Families;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Locomotion;
using ChickenAPI.Game.Quicklist;
using ChickenAPI.Game.Shops;
using ChickenAPI.Game.Specialists;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public interface IPlayerEntity : ILocomotionEntity, IBattleEntity, IInventoriedEntity, IShopEntity, ISpecialistEntity, IQuicklistEntity, IFamilyEntity, IGroupEntity, IBroadcastable
    {
        IRelationList Relations { get; }


        CharacterDto Character { get; }

        CharacterNameAppearance NameAppearance { get; }

        ISession Session { get; }
        PersonalShop Shop { get; set; }

        DateTime DateLastPortal { get; set; }

        double LastPortal { get; set; }

        List<IMateEntity> Mates { get; set; }
        IEnumerable<IMateEntity> ActualMates { get; }

        /// <summary>
        /// </summary>
        long LastPulse { get; }

        Task BroadcastExceptSenderAsync<T>(T packet) where T : IPacket;
        Task BroadcastExceptSenderAsync<T>(IEnumerable<T> packets) where T : IPacket;

        Task SendPacketAsync<T>(T packet) where T : IPacket;

        Task SendPacketsAsync<T>(IEnumerable<T> packets) where T : IPacket;

        Task SendPacketsAsync(IEnumerable<IPacket> packets);

        /// <summary>
        ///     Saves player's state
        /// </summary>
        void Save();

        #region Equipments

        ItemInstanceDto Fairy { get; }
        ItemInstanceDto Weapon { get; }
        ItemInstanceDto SecondaryWeapon { get; }
        ItemInstanceDto Armor { get; }

        #endregion Equipments
    }
}