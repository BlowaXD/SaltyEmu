using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Core.IPC;
using ChickenAPI.Core.IPC.Protocol;

namespace SaltyEmu.Communication.Routing
{
    public class RuntimeSmartIpcRouter : IIpcPacketRouter
    {
        private readonly IRoutingInformationFactory _routingInformationFactory;
        private readonly Dictionary<Type, IRoutingInformation> _infos = new Dictionary<Type, IRoutingInformation>();

        public RuntimeSmartIpcRouter(IRoutingInformationFactory routingInformationFactory)
        {
            _routingInformationFactory = routingInformationFactory;
        }

        private const string RequestTopicSuffix = "/requests";
        private const string ResponseTopicSuffix = "/responses";
        private const string PacketTopicSuffix = "/broadcasts";
        
        private async Task<IRoutingInformation> RegisterGenericRequestAsync(Type type)
        {
            // in case they want to register "runtime" generics objects
            Type evaluatedType = type.BaseType ?? type;

            var newTopic = new StringBuilder();
            newTopic.Append(type.Name);

            foreach (Type i in evaluatedType.GenericTypeArguments)
            {
                newTopic.AppendFormat("/{0}", i.Name.ToLower());
            }

            string requestTopic = newTopic.ToString();
            string responseTopic = "";

            if (type.GetInterfaces().Any(s => s == typeof(IIpcRequest)))
            {
                requestTopic += RequestTopicSuffix;
                responseTopic = newTopic + ResponseTopicSuffix;
            }
            else if (type.GetInterfaces().Any(s => s == typeof(IIpcPacket)))
            {
                requestTopic += PacketTopicSuffix;
            }

            IRoutingInformation routingInfos = await _routingInformationFactory.Create(requestTopic, responseTopic);
            await RegisterAsync(type, routingInfos);
            return routingInfos;
        }

        public async Task<IRoutingInformation> RegisterAsync(Type type)
        {
            if (type.IsGenericType)
            {
                return await RegisterGenericRequestAsync(type);
            }

            string requestTopic = "";
            string responseTopic = "";
            if (type.GetInterfaces().Any(s => s == typeof(IIpcRequest)))
            {
                requestTopic = type.Name.ToLower() + RequestTopicSuffix;
                responseTopic = type.Name.ToLower() + ResponseTopicSuffix;
            }
            else if (type.GetInterfaces().Any(s => s == typeof(IIpcPacket)))
            {
                requestTopic = type.Name.ToLower() + PacketTopicSuffix;
            }

            IRoutingInformation routingInfos = await _routingInformationFactory.Create(requestTopic, responseTopic);
            await RegisterAsync(type, routingInfos);
            return routingInfos;
        }

        public Task RegisterAsync(Type type, IRoutingInformation routingInformation)
        {
            if (_infos.ContainsKey(type))
            {
                return Task.CompletedTask;
            }

            _infos.Add(type, routingInformation);
            return Task.CompletedTask;
        }

        public async Task<IRoutingInformation> GetRoutingInformationsAsync(Type type)
        {
            if (_infos.TryGetValue(type, out IRoutingInformation value))
            {
                return value;
            }

            return await RegisterAsync(type);
        }
    }
}