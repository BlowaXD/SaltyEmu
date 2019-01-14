namespace SaltyEmu.Database.MongoDB
{
    public class MongoConfigurationBuilder
    {
        private string _databaseName;
        private string _connectionString;

        public MongoConfigurationBuilder ConnectTo(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        public MongoConfigurationBuilder WithDatabaseName(string databaseName)
        {
            _databaseName = databaseName;
            return this;
        }

        public MongoConfiguration Build()
        {
            return new MongoConfiguration
            {
            };
        }
    }
}