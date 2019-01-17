using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.IPC;
using SaltyEmu.Communication.Communicators;
using SaltyEmu.Communication.Routing;
using SaltyEmu.Communication.Serializers;

namespace SaltyEmu.Communication.Utils
{
    public class CommunicationIocInjector
    {
        public static void Inject()
        {
            ChickenContainer.Builder.RegisterType<RoutingInformationFactory>().As<IRoutingInformationFactory>().PropertiesAutowired().InstancePerDependency();
            ChickenContainer.Builder.RegisterType<PendingRequestFactory>().As<IPendingRequestFactory>().PropertiesAutowired().InstancePerDependency();
            ChickenContainer.Builder.RegisterType<JsonSerializer>().As<IIpcSerializer>().PropertiesAutowired().InstancePerDependency();

            ChickenContainer.Builder.RegisterType<RuntimeSmartIpcRouter>().As<IIpcPacketRouter>().PropertiesAutowired().SingleInstance();
            ChickenContainer.Builder.RegisterType<PacketHandlersContainer>().As<IIpcPacketHandlersContainer>().PropertiesAutowired().SingleInstance();
            ChickenContainer.Builder.RegisterType<MqttIpcClient>().As<IIpcClient>().PropertiesAutowired().SingleInstance();
            ChickenContainer.Builder.RegisterType<MqttIpcServer>().As<IIpcServer>().PropertiesAutowired().SingleInstance();
        }
    }
}