using System.Collections.Generic;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Data.TransferObjects.Server;

namespace ChickenAPI.Game.Data.AccessLayer.Friends
{
    public interface IFriendService
    {
        void Register(CharacterDto character, PlayerSessionDto session);

        void UnregisterByCharacterId(long characterId);

        /// <summary>
        ///     Gets all the online friends within the server group
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        IEnumerable<PlayerSessionDto> GetFriendsOnline(CharacterDto character);

        /// <summary>
        ///     Sends a message to character
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receiverCharacterId"></param>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        void SendMessageToCharacter(PlayerSessionDto sender, long receiverCharacterId, string message, MessageType messageType);
    }

    public enum MessageType
    {
        Whisper,
        FriendTalk
    }
}