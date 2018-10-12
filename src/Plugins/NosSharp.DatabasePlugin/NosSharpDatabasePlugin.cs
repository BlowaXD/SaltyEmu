using System;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Utils;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Configuration;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Utils;

namespace SaltyEmu.DatabasePlugin
{
    public class NosSharpDatabasePlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<NosSharpDatabasePlugin>();
        private readonly string _configurationFilePath = $"plugins/config/{nameof(NosSharpDatabasePlugin)}/conf.json";
        private DatabaseConfiguration _configuration;
        public string Name => nameof(NosSharpDatabasePlugin);

        public void OnLoad()
        {
            Log.Info("Loading...");
            _configuration = ConfigurationHelper.Load<DatabaseConfiguration>(_configurationFilePath, true); // database configuration
            if (!Initialize())
            {
                return;
            }

            PluginDependencyInjector.RegisterDbContext(_configuration);
            PluginDependencyInjector.RegisterDependencies();
            Log.Info("Loaded !");
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


        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }

        private bool Initialize()
        {
            using (var context = new SaltyDbContext(new DbContextOptionsBuilder<SaltyDbContext>().UseSqlServer(_configuration.ToString()).Options))
            {
                try
                {
                    context.Database.Migrate();
                    Log.Info("DATABASE_INITIALIZED");
                }
                catch (Exception ex)
                {
                    Log.Error("Database Error", ex);
                    return false;
                }

                return true;
            }
        }
    }
}