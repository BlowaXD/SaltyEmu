using System;
using System.Data.SqlClient;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Plugins.Exceptions;
using ChickenAPI.Core.Utils;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Configuration;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Utils;

namespace SaltyEmu.DatabasePlugin
{
    public class DatabasePlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<DatabasePlugin>();
        private readonly string _configurationFilePath = $"plugins/config/{nameof(DatabasePlugin)}/conf.json";
        private DatabaseConfiguration _configuration;
        public PluginEnableTime EnableTime => PluginEnableTime.PreContainerBuild;
        public string Name => nameof(DatabasePlugin);

        public void OnLoad()
        {
            Log.Info("Loading...");
            _configuration = ConfigurationHelper.Load<DatabaseConfiguration>(_configurationFilePath, true); // database configuration
            if (!Initialize())
            {
                throw new CriticalPluginException(this, "Verify your configuration in : " + _configurationFilePath);
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
                    switch (ex)
                    {
                        case SqlException sql:
                            return false;
                    }

                    Log.Error("[DB_INIT]", ex);
                    return false;
                }

                return true;
            }
        }
    }
}