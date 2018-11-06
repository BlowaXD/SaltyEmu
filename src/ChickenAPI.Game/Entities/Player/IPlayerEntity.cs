using System.Collections.Generic;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Families;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Permissions;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public interface IPlayerEntity : IBattleEntity, IInventoriedEntity, IExperenciedEntity, ISpecialistEntity, IQuicklistEntity, IFamilyCapacities, IBroadcastable
    {
        CharacterDto Character { get; }

        ISession Session { get; }

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