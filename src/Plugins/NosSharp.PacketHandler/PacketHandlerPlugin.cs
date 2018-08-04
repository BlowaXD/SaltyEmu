using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Game.Packets;
using NosSharp.PacketHandler.Utils;

namespace NosSharp.PacketHandler
{
    public class PacketHandlerPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<PacketHandlerPlugin>();
        public string Name => "Nos#-PacketHandler";

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            try
            {
                var packetHandler = Container.Instance.Resolve<IPacketHandler>();
                foreach (CharacterScreenPacketHandler handler in PacketMethodGenerator.GetCharacterScreenPacketHandlers())
                {
                    Log.Info($"[PACKET_ADD][CHARACTERSCREEN] {handler.Identification}...");
                    packetHandler.Register(handler);
                }
                foreach (GamePacketHandler handler in PacketMethodGenerator.GetGamePacketHandlers())
                {
                    Log.Info($"[PACKET_ADD][GAME] {handler.Identification}...");
                    packetHandler.Register(handler);
                }
            }
            catch (Exception e)
            {
                Log.Error("OnEnable", e);
                throw;
            }
        }

        public void OnLoad()
        {
            Log.Info("Loading...");
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