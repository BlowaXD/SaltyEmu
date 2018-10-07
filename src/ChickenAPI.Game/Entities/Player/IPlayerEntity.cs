using System.Collections.Generic;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Permissions;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public interface IPlayerEntity : IBattleEntity, IInventoriedEntity, IExperenciedEntity, ISkillEntity, ISpecialistEntity, IQuicklistEntity
    {
        CharacterDto Character { get; }
        ISession Session { get; }

        bool HasPermission(PermissionType permission);
        bool HasPermission(string permissionKey);
        bool HasPermission(PermissionsRequirementsAttribute permissions);

        long LastPulse { get; }

        void Broadcast<T>(T packet) where T : IPacket;
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
        void Save();
    }
}