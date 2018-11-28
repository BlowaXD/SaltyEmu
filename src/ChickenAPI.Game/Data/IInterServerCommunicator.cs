using System;
using System.Threading.Tasks;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Data
{
    public interface IInterServerCommunicator
    {
        Task WhisperPlayerAsync(string senderName, string targetName, string message);

        Task BroadcastPacketAsync<T>(T packet) where T : IPacket;
    }
}