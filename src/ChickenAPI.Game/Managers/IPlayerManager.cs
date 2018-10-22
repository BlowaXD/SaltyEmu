using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Managers
{
    public interface IPlayerManager
    {
        IPlayerEntity GetPlayerByCharacterName(string characterName);
        IPlayerEntity GetPlayerByCharacterId(long characterId);

        /// <summary>
        /// Registers a player into the manager
        /// </summary>
        /// <param name="player"></param>
        void RegisterPlayer(IPlayerEntity player);

        /// <summary>
        /// Unregister the player from the manager
        /// </summary>
        /// <param name="player"></param>
        void UnregisterPlayer(IPlayerEntity player);
    }
}