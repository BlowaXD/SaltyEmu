using System;
using Autofac;
using ChickenAPI.Data.AccessLayer;
using ChickenAPI.Data.AccessLayer.Server;
using ChickenAPI.Plugins;
using ChickenAPI.Utils;

namespace NosSharp.RedisSessionPlugin
{
    public class RedisPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<RedisPlugin>();
        private readonly string _configurationPath = $"plugins/config/{nameof(RedisPlugin)}/conf.json";
        private RedisConfiguration _configuration;
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
            Log.Info($"Loading...");
            _configuration = ConfigurationHelper.Load<RedisConfiguration>(_configurationPath, true);
            Container.Builder.Register(s => new RedisSessionService(_configuration)).As<ISessionService>();
            Log.Info($"ISessionService registered !");
            Container.Builder.Register(s => new RedisServerApi(_configuration)).As<IServerApiService>();
            Log.Info($"IServerApiService registered !");
            Container.Builder.Register(s => new RedisLanguageService(_configuration)).As<ILanguageService>();
            Log.Info($"ILanguageService registered !");
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