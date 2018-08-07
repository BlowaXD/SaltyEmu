using System;
using System.Data;
using NpgsqlTypes;

namespace NosSharp.DatabasePlugin.Configuration
{
    public class DatabaseConfiguration
    {
        public DatabaseConfiguration()
        {
            Ip = "localhost";
            Username = "db-user";
            Password = "DevNos=2018";
            Database = "saltyemu";
            Port = 1433;
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