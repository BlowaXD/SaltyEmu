using System;
using Autofac;
using ChickenAPI.Core.Configurations;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Server;
using ChickenAPI.Game._i18n;
using ChickenAPI.Game.Impl;
using SaltyEmu.Redis;
using SaltyEmu.RedisWrappers.Redis;

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
            throw new NotImplementedException();
        }

        public void OnEnable()
        {
            // hehe
        }

        public void OnLoad()
        {
            IConfigurationManager conf = new ConfigurationHelper(new JsonConfigurationSerializer());
            _configuration = conf.Load<RedisConfiguration>(_configurationPath, true);
            ChickenContainer.Builder.Register(s => new RedisSessionService(_configuration)).As<ISessionService>();
            ChickenContainer.Builder.Register(s => new RedisServerApi(_configuration)).As<IServerApiService>();
            ChickenContainer.Builder.Register(s => new RedisGameLanguageService(_configuration)).As<IGameLanguageService>();
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