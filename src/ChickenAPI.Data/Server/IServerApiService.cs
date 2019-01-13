using System;
using System.Collections.Generic;

namespace ChickenAPI.Data.Server
{
    public interface IServerApiService
    {
        /// <summary>
        ///     Gets the running server from where you call that method
        /// </summary>
        WorldServerDto GetRunningServer { get; }

        /// <summary>
        ///     Register the server in the global server list
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        bool RegisterServer(WorldServerDto dto);

        /// <summary>
        ///     Unregister the server from global server list
        /// </summary>
        /// <param name="id"></param>
        void UnregisterServer(Guid id);

        /// <summary>
        ///     Retrieve all servers from global server list
        /// </summary>
        /// <returns></returns>
        IEnumerable<WorldServerDto> GetServers();
    }
}