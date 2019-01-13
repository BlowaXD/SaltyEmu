using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Managers
{
    public interface IPlayerManager
    {
        /// <summary>
        ///     Retrieves the PlayerEntity by its character's name
        /// </summary>
        /// <param name="characterName"></param>
        /// <returns></returns>
        IPlayerEntity GetPlayerByCharacterName(string characterName);

        IPlayerEntity GetPlayerByCharacterId(long characterId);

        /// <summary>
        ///     Registers a player into the manager
        /// </summary>
        /// <param name="player"></param>
        void RegisterPlayer(IPlayerEntity player);

        /// <summary>
        ///     Unregister the player from the manager
        /// </summary>
        /// <param name="player"></param>
        void UnregisterPlayer(IPlayerEntity player);

        Task BroadcastAsync<T>(IEnumerable<T> packets) where T : IPacket;
        Task BroadcastAsync<T>(T packet) where T : IPacket;

        Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket;
        Task BroadcastAsync<T>(T packet, IBroadcastRule rule) where T : IPacket;
    }
}