using System;
using System.Collections.Generic;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Families;
using ChickenAPI.Game.Locomotion;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Player;
using ChickenAPI.Game.Specialists;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public interface IPlayerEntity : ILocomotionEntity, IBattleEntity, IInventoriedEntity, IExperenciedEntity, ISpecialistEntity, IQuicklistEntity, IFamilyCapacities, IGroupEntity, IBroadcastable
    {
        CharacterDto Character { get; }

        CharacterNameAppearance NameAppearance { get; }

        ISession Session { get; }

        DateTime DateLastPortal { get; set; }

        double LastPortal { get; set; }

        #region Equipments

        ItemInstanceDto Fairy { get; }
        ItemInstanceDto Weapon { get; }
        ItemInstanceDto SecondaryWeapon { get; }
        ItemInstanceDto Armor { get; }

        #endregion


        /// <summary>
        ///
        /// </summary>
        long LastPulse { get; }

        /// <summary>
        /// Broadcasts to every players on the current map except the sender
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        void BroadcastExceptSender<T>(T packet) where T : IPacket;

        /// <summary>
        /// Broadcasts to every players on the current map except the sender
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packets"></param>
        void BroadcastExceptSender<T>(IEnumerable<T> packets) where T : IPacket;

        void SendPacket<T>(T packetBase) where T : IPacket;

        void SendPackets<T>(IEnumerable<T> packets) where T : IPacket;

        void SendPackets(IEnumerable<IPacket> packets);

        /// <summary>
        /// Saves player's state
        /// </summary>
        void Save();
    }
}