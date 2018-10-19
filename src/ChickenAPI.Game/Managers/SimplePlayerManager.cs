using System.Collections.Generic;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Managers
{
    public class SimplePlayerManager : IPlayerManager
    {
        private readonly Dictionary<long, IPlayerEntity> _playersByCharacterId = new Dictionary<long, IPlayerEntity>();
        private readonly Dictionary<string, IPlayerEntity> _playersByCharacterName = new Dictionary<string, IPlayerEntity>();
        public IPlayerEntity GetPlayerByCharacterName(string characterName) => !_playersByCharacterName.TryGetValue(characterName, out IPlayerEntity player) ? null : player;

        public IPlayerEntity GetPlayerByCharacterId(long characterId) => !_playersByCharacterId.TryGetValue(characterId, out IPlayerEntity player) ? null : player;
        public void RegisterPlayer(IPlayerEntity player)
        {
            RegisterPlayer(player.Character.Id, player);
            RegisterPlayer(player.Character.Name, player);
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

        public void UnregisterPlayer(IPlayerEntity player)
        {
            UnregisterPlayer(player.Character.Id);
            UnregisterPlayer(player.Character.Name);
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