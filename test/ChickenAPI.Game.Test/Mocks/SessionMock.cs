using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ChickenAPI.Core.i18n;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Old;
using Moq;

namespace ChickenAPI.Game.Test.Mocks
{
    public class SessionMock : ISession
    {
        private readonly ISession _session;
        private readonly List<Tuple<Type, IPacket>> _packets = new List<Tuple<Type, IPacket>>();

        public SessionMock()
        {
            Mock<ISession> mock = new Mock<ISession>();
            mock.Setup(_ => _.Account).Returns(new AccountDto
            {
                Authority = AuthorityType.User
            });
            _session = mock.Object;
        }

        public int SessionId => _session.SessionId;

        public long CharacterId => _session.CharacterId;

        public bool IsAuthenticated => _session.IsAuthenticated;

        public IPAddress Ip => _session.Ip;

        public AccountDto Account => _session.Account;

        public IPlayerEntity Player => _session.Player;

        public LanguageKey Language => _session.Language;

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
            _packets.Add(new Tuple<Type, IPacket>(typeof(T), packet));
            _session.SendPacketAsync(packet);
        }

        public IReadOnlyList<Tuple<Type, IPacket>> Packets => _packets;

        public void SendPackets<T>(IEnumerable<T> packets) where T : IPacket
        {
            foreach (T i in packets)
            {
                SendPacket(i);
            }
        }

        public void SendPackets(IEnumerable<IPacket> packets)
        {
            foreach (IPacket packet in packets)
            {
                _packets.Add(new Tuple<Type, IPacket>(packet.GetType(), packet));
                _session.SendPacketAsync(packet);
            }
        }

        public Task SendPacketAsync<T>(T packet) where T : IPacket => throw new NotImplementedException();

        public Task SendPacketsAsync<T>(IEnumerable<T> packets) where T : IPacket => throw new NotImplementedException();

        public Task SendPacketsAsync(IEnumerable<IPacket> packets) => throw new NotImplementedException();

        public void Disconnect()
        {
            _session.Disconnect();
        }
    }
}