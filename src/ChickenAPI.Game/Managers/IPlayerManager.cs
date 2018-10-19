using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Managers
{
    public interface IPlayerManager
    {
        IPlayerEntity GetPlayerByCharacterName(string characterName);
        IPlayerEntity GetPlayerByCharacterId(long characterId);

        void RegisterPlayer(IPlayerEntity player);
        void UnregisterPlayer(IPlayerEntity player);
    }
}