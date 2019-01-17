using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Managers
{
    public class SimplePlayerManager : IPlayerManager
    {
        private readonly object _locker = new object();
        private readonly HashSet<IPlayerEntity> _players = new HashSet<IPlayerEntity>();
        private readonly Dictionary<long, IPlayerEntity> _playersByCharacterId = new Dictionary<long, IPlayerEntity>();
        private readonly Dictionary<string, IPlayerEntity> _playersByCharacterName = new Dictionary<string, IPlayerEntity>();
        public IPlayerEntity GetPlayerByCharacterName(string characterName) => !_playersByCharacterName.TryGetValue(characterName, out IPlayerEntity player) ? null : player;

        public IPlayerEntity GetPlayerByCharacterId(long characterId) => !_playersByCharacterId.TryGetValue(characterId, out IPlayerEntity player) ? null : player;

        public void RegisterPlayer(IPlayerEntity player)
        {
            lock(_locker)
            {
                RegisterPlayer(player.Character.Id, player);
                RegisterPlayer(player.Character.Name, player);
                _players.Add(player);
            }
        }

        public void UnregisterPlayer(IPlayerEntity player)
        {
            lock(_locker)
            {
                UnregisterPlayer(player.Character.Id);
                UnregisterPlayer(player.Character.Name);
                _players.Remove(player);
            }
        }

        public Task BroadcastAsync<T>(IEnumerable<T> packets) where T : IPacket
        {
            lock(_locker)
            {
                return Task.WhenAll(_players.Select(s => Task.Run(() => s.SendPacketsAsync(packets))));
            }
        }

        public Task BroadcastAsync<T>(T packet) where T : IPacket
        {
            lock(_locker)
            {
                return Task.WhenAll(_players.Select(s => Task.Run(() => s.SendPacketAsync(packet))));
            }
        }

        public Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket
        {
            lock(_locker)
            {
                return Task.WhenAll(_players.Select(s => Task.Run(() => rule != null && rule.Match(s) ? s.SendPacketsAsync(packets) : Task.CompletedTask)));
            }
        }

        public Task BroadcastAsync(IEnumerable<IPacket> packets) => BroadcastAsync(packets, null);

        public Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule)
        {
            lock(_locker)
            {
                return Task.WhenAll(_players.Select(s => Task.Run(() => rule != null && rule.Match(s) ? s.SendPacketsAsync(packets) : Task.CompletedTask)));
            }
        }

        public Task BroadcastAsync<T>(T packet, IBroadcastRule rule) where T : IPacket
        {
            lock(_locker)
            {
                return Task.WhenAll(_players.Select(s => Task.Run(() => rule != null && rule.Match(s) ? s.SendPacketAsync(packet) : Task.CompletedTask)));
            }
        }

        private void RegisterPlayer(long characterId, IPlayerEntity player)
        {
            if (_playersByCharacterId.ContainsKey(characterId))
            {
                return;
            }

            _playersByCharacterId.Add(characterId, player);
        }

        private void RegisterPlayer(string characterName, IPlayerEntity player)
        {
            if (_playersByCharacterName.ContainsKey(characterName))
            {
                return;
            }

            _playersByCharacterName.Add(characterName, player);
        }

        private void UnregisterPlayer(string characterName)
        {
            _playersByCharacterName.Remove(characterName);
        }

        private void UnregisterPlayer(long characterId)
        {
            _playersByCharacterId.Remove(characterId);
        }
    }
}