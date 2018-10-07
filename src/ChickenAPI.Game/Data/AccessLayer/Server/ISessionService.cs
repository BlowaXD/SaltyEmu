using System;
using ChickenAPI.Data.Server;

namespace ChickenAPI.Game.Data.AccessLayer.Server
{
    public interface ISessionService
    {
        /// <summary>
        ///     Register the session in the SessionService
        /// </summary>
        /// <param name="dto"></param>
        void RegisterSession(PlayerSessionDto dto);

        /// <summary>
        ///     Update the session values in the SessionService
        /// </summary>
        /// <param name="dto"></param>
        void UpdateSession(PlayerSessionDto dto);

        /// <summary>
        ///     Returns the session associated to the accountName
        /// </summary>
        /// <param name="accountName"></param>
        /// <returns></returns>
        PlayerSessionDto GetByAccountName(string accountName);

        /// <summary>
        ///     Returns the session associated to sessionId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PlayerSessionDto GetBySessionId(int id);

        /// <summary>
        ///     Unregister the session from SessionService
        /// </summary>
        /// <param name="sessionId"></param>
        void UnregisterSession(int sessionId);

        /// <summary>
        ///     Unregister all the session connected on the server with ServerId
        /// </summary>
        /// <param name="serverId"></param>
        void UnregisterSessions(Guid serverId);
    }
}