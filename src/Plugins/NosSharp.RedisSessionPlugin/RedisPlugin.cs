using System;
using Autofac;
using ChickenAPI.Core.Configurations;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Data.Server;
using ChickenAPI.Game._i18n;
using ChickenAPI.Game.Impl;
using SaltyEmu.Redis;

namespace SaltyEmu.RedisWrappers
{
    public class RedisPlugin : IPlugin
    {
        private readonly string _configurationPath = $"plugins/config/{nameof(RedisPlugin)}/conf.json";
        private RedisConfiguration _configuration;
        public PluginEnableTime EnableTime => PluginEnableTime.PreContainerBuild;
        public string Name => "RedisPlugin";

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            // hehe
        }

        public void OnLoad()
        {
            IConfigurationManager conf = new ConfigurationHelper(new JsonConfigurationSerializer());
            _configuration = conf.Load<RedisConfiguration>(_configurationPath, true);
            ChickenContainer.Builder.Register(s => _configuration).As<RedisConfiguration>();
            ChickenContainer.Builder.RegisterType<RedisSessionService>().As<ISessionService>();
            ChickenContainer.Builder.RegisterType<RedisServerApi>().As<IServerApiService>();
            ChickenContainer.Builder.RegisterType<RedisGameLanguageService>().As<IGameLanguageService>();
        }

        public void ReloadConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveDefaultConfig()
        {
            throw new NotImplementedException();
        }
    }
}