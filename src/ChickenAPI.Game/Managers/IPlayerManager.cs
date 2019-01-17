using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game._Network;

namespace ChickenAPI.Game.Managers
{
    public interface IPlayerManager : IBroadcastable
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
    }
}