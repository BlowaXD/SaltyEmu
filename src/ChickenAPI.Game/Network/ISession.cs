using System.Collections.Generic;
using System.Net;
using ChickenAPI.Core.i18n;
using ChickenAPI.Data.Character;
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

        void SendPacket<T>(T packet) where T : IPacket;
        void SendPackets<T>(IEnumerable<T> packets) where T : IPacket;
        void SendPackets(IEnumerable<IPacket> packets);

        void Disconnect();
    }
}