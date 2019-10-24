using System;
using ChickenAPI.Core.Configurations;

namespace SaltyEmu.DatabasePlugin.Configuration
{
    public class DatabaseConfiguration : IConfiguration
    {
        public DatabaseConfiguration()
        {
            Ip = Environment.GetEnvironmentVariable("DATABASE_IP") ?? "localhost";
            Username = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "sa";
            Password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "strong_pass2018";
            Database = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "saltyemu";
            if (!ushort.TryParse(Environment.GetEnvironmentVariable("DATABASE_PORT") ?? "1433", out ushort port))
            {
                port = 1433;
            }

            Port = port;
            Type = DbProviderType.MSSQL;
        }

        public string Ip { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public ushort Port { get; set; }
        public DbProviderType Type { get; set; }

        public override string ToString()
        {
            switch (Type)
            {
                case DbProviderType.MSSQL:
                    return $"Server={Ip},{Port};User id={Username};Password={Password};Initial Catalog={Database};";
                case DbProviderType.PostgreSQL:
                    return $"Host={Ip};Port={Port};Username={Username};Password={Password};Database={Database}";
                case DbProviderType.MySQL:
                    return "";
                default:
                    return "";
            }
        }
    }
}