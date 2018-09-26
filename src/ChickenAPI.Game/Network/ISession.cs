using System.Collections.Generic;
using System.Net;
using ChickenAPI.Core.i18n;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Network
{
    public interface ISession
    {
        int SessionId { get; }

        long CharacterId { get; }

        bool IsAuthenticated { get; }

        IPAddress Ip { get; }

        AccountDto Account { get; }
        IPlayerEntity Player { get; }
        LanguageKey Langage { get; }

        void InitializeAccount(AccountDto dto);
        void InitializeCharacterId(long id);
        void InitializeEntity(IPlayerEntity ett);

        /// <summary>
        ///     Sends to every connected clients in the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packet"></param>
        void GlobalBroadcast<T>(T packet) where T : IPacket;

        /// <summary>
        ///     Sends to every connected clients in the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="packets"></param>
        void GlobalBroadcast<T>(IEnumerable<T> packets) where T : IPacket;

        void SendPacket<T>(T packet) where T : IPacket;
        void SendPackets<T>(IEnumerable<T> packets) where T : IPacket;
        void SendPackets(IEnumerable<IPacket> packets);

        void Disconnect();
    }
}