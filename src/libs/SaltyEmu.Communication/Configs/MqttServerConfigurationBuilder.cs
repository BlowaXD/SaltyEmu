namespace SaltyEmu.Communication.Configs
{
    public class MqttServerConfigurationBuilder
    {
        private string _endpoint;
        private string _clientName;

        public MqttServerConfigurationBuilder WithName(string name)
        {
            _clientName = name;
            return this;
        }

        public MqttServerConfigurationBuilder ConnectTo(string endpoint)
        {
            _endpoint = endpoint;
            return this;
        }
        
        public MqttIpcServerConfiguration Build()
        {
            return new MqttIpcServerConfiguration
            {
                ClientName = _clientName,
                EndPoint= _endpoint,
            };
        }

        public static implicit operator MqttIpcServerConfiguration(MqttServerConfigurationBuilder builder)
        {
            return builder.Build();
        }
    }
}