using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Data.Relations;
using SaltyEmu.Communication.Configs;
using SaltyEmu.Communication.Serializers;
using SaltyEmu.Communication.Utils;
using SaltyEmu.FriendsPlugin.Services;

namespace SaltyEmu.FriendsPlugin
{

    public class RelationsPlugin : IPlugin
    {
        public PluginEnableTime EnableTime => PluginEnableTime.PreContainerBuild;
        public string Name => nameof(RelationsPlugin);

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            MqttClientConfigurationBuilder builder = new MqttClientConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("relations-client")
                .WithRequestTopic(Configuration.RequestQueue)
                .WithBroadcastTopic(Configuration.BroadcastQueue)
                .WithResponseTopic(Configuration.ResponseQueue)
                .WithSerializer(new JsonSerializer());
            var tmp = new RelationServiceClient(builder);
            MqttServerConfigurationBuilder serverBuider = new MqttServerConfigurationBuilder()
                .ConnectTo("localhost")
                .WithName("relations-server-world")
                .WithRequestHandler(new RequestHandler())
                .AddTopic(Configuration.RequestQueue)
                .WithBroadcastTopic(Configuration.BroadcastQueue)
                .WithResponseTopic(Configuration.ResponseQueue)
                .WithSerializer(new JsonSerializer());

            var serve = new RelationServiceGameServer(serverBuider);

            tmp.InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            serve.InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            ChickenContainer.Builder.Register(s => tmp).As<IRelationService>().SingleInstance();
        }

        public void OnLoad()
        {
        }

        public void ReloadConfig()
        {
        }

        public void SaveConfig()
        {
        }

        public void SaveDefaultConfig()
        {
        }
    }
}