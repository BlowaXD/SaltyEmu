namespace SaltyEmu.Database.MongoDB
{
    public class MongoConfigurationBuilder
    {
        private string _databaseName;
        private string _endpoint;
        private short _port;

        public MongoConfigurationBuilder ConnectTo(string ip)
        {
            _endpoint = ip;
            return this;
        }

        public MongoConfigurationBuilder WithDatabaseName(string databaseName)
        {
            _databaseName = databaseName;
            return this;
        }

        public MongoConfigurationBuilder WithPort(short port)
        {
            _port = port;
            return this;
        }

        public MongoConfiguration Build()
        {
            return new MongoConfiguration
            {
                Endpoint = _endpoint,
                Port = _port,
                DatabaseName = _databaseName
            };
        }
    }
}