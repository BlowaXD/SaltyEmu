using System;
using System.Collections.Generic;
using ChickenAPI.Data.TransferObjects.Server;

namespace ChickenAPI.Data.AccessLayer.Server
{
    public interface IServerApiService
    {
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