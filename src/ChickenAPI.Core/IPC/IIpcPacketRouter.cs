using System;
using System.Threading.Tasks;

namespace ChickenAPI.Core.IPC
{
    public interface IIpcPacketRouter
    {
        /// <summary>
        /// Will register and generate the default routing informations related to Router's configuration
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IRoutingInformation> RegisterAsync(Type type);

        /// <summary>
        /// Will register the packet routing information for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="routingInformation"></param>
        /// <returns></returns>
        Task RegisterAsync(Type type, IRoutingInformation routingInformation);

        /// <summary>
        /// Get the routing informations from the router
        /// re
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IRoutingInformation> GetRoutingInformationsAsync(Type type);
    }
}