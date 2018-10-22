using System.Collections.Generic;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Families;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Permissions;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public interface IPlayerEntity : IBattleEntity, IInventoriedEntity, IExperenciedEntity, ISkillEntity, ISpecialistEntity, IQuicklistEntity, IFamilyCapacities
    {
        CharacterDto Character { get; }

        ISession Session { get; }

        /// <summary>
        /// Checks if the player has the given permission
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        bool HasPermission(PermissionType permission);

        /// <summary>
        /// Checks if the player has the given permission
        /// </summary>
        /// <param name="permissionKey"></param>
        /// <returns></returns>
        bool HasPermission(string permissionKey);

        /// <summary>
        /// Checks f the player has the given permissions
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        bool HasPermission(PermissionsRequirementsAttribute permissions);

        /// <summary>
        /// 
        /// </summary>
        long LastPulse { get; }

        /// <summary>
        /// Broadcasts the packet and receives the packet too
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        void Broadcast<T>(T packet) where T : IPacket;

        /// <summary>
        /// Broadcasts all the packets and receives it as well
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packets"></param>
        void Broadcast<T>(IEnumerable<T> packets) where T : IPacket;

        /// <summary>
        /// Broadcast a packet to every entities in the same MapLayer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        /// <param name="doNotReceive">Player won't receive the packet if true</param>
        void Broadcast<T>(T packet, bool doNotReceive) where T : IPacket;

        /// <summary>
        /// Broadcasts a packet to every entities in the same MapLayer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packets"></param>
        /// <param name="doNotReceive"></param>
        void Broadcast<T>(IEnumerable<T> packets, bool doNotReceive) where T : IPacket;

        void SendPacket<T>(T packetBase) where T : IPacket;

        void SendPackets<T>(IEnumerable<T> packets) where T : IPacket;

        void SendPackets(IEnumerable<IPacket> packets);


        /// <summary>
        /// Saves player's state
        /// </summary>
        void Save();
    }
}