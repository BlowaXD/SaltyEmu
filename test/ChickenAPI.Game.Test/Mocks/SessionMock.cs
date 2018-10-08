using System.Collections.Generic;
using System.Net;
using ChickenAPI.Core.i18n;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets;
using Moq;

namespace ChickenAPI.Game.Test.Mocks
{
    public class SessionMock : ISession
    {
        private readonly ISession _session;
        public SessionMock()
        {
            Mock<ISession> mock = new Mock<ISession>();
            _session = mock.Object;
        }

        public int SessionId => _session.SessionId;

        public long CharacterId => _session.CharacterId;

        public bool IsAuthenticated => _session.IsAuthenticated;

        public IPAddress Ip => _session.Ip;

        public AccountDto Account => _session.Account;

        public IPlayerEntity Player => _session.Player;

        public LanguageKey Langage => _session.Langage;

        public void InitializeAccount(AccountDto dto)
        {
            _session.InitializeAccount(dto);
        }

        public void InitializeCharacterId(long id)
        {
            _session.InitializeCharacterId(id);
        }

        public void InitializeEntity(IPlayerEntity ett)
        {
            _session.InitializeEntity(ett);
        }

        public void SendPacket<T>(T packet) where T : IPacket
        {
            _session.SendPacket(packet);
        }

        public void SendPackets<T>(IEnumerable<T> packets) where T : IPacket
        {
            _session.SendPackets(packets);
        }

        public void SendPackets(IEnumerable<IPacket> packets)
        {
            _session.SendPackets(packets);
        }

        public void Disconnect()
        {
            _session.Disconnect();
        }
    }
}