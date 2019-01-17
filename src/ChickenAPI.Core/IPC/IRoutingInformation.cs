using System.Threading.Tasks;

namespace ChickenAPI.Core.IPC
{
    public interface IRoutingInformationFactory
    {
        Task<IRoutingInformation> Create(string topic, string responseTopic);
    }

    public interface IRoutingInformation
    {
        string Topic { get; }
        string ResponseTopic { get; }
    }
}