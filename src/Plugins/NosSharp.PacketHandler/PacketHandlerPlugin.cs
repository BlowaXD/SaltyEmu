using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game._Network;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling
{
    public class PacketHandlerPlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<PacketHandlerPlugin>();
        public PluginEnableTime EnableTime => PluginEnableTime.PostContainerBuild;
        public string Name => "SaltyEmu";

        public void OnDisable()
        {
        }

        public void OnEnable()
        {
            try
            {
                var packetPipeline = ChickenContainer.Instance.Resolve<IPacketPipelineAsync>();

                foreach (Type handlerType in typeof(PacketHandlerPlugin).Assembly.GetTypesImplementingGenericClass(typeof(GenericSessionPacketHandlerAsync<>)))
                {
                    try
                    {
                        object tmp = ChickenContainer.Instance.Resolve(handlerType);
                        if (!(tmp is IPacketProcessor handler))
                        {
                            continue;
                        }

                        Type type = handlerType.BaseType.GenericTypeArguments[0];

                        Log.Info($"[CHARACTER_SCREEN][HANDLING] {handlerType}");
                        packetPipeline.RegisterAsync(handler, type).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }

                foreach (Type handlerType in typeof(PacketHandlerPlugin).Assembly.GetTypesImplementingGenericClass(typeof(GenericGamePacketHandlerAsync<>)))
                {
                    try
                    {
                        object tmp = ChickenContainer.Instance.Resolve(handlerType);
                        if (!(tmp is IPacketProcessor handler))
                        {
                            continue;
                        }

                        Type type = handlerType.BaseType.GenericTypeArguments[0];

                        Log.Info($"[GAME][HANDLING] {handlerType}");
                        packetPipeline.RegisterAsync(handler, type).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("OnEnable", e);
            }
        }

        public void OnLoad()
        {
            Log.Info("Loading...");
            ChickenContainer.Builder.RegisterAssemblyTypes(typeof(PacketHandlerPlugin).Assembly).AsClosedTypesOf(typeof(GenericSessionPacketHandlerAsync<>)).PropertiesAutowired();
            ChickenContainer.Builder.RegisterAssemblyTypes(typeof(PacketHandlerPlugin).Assembly).AsClosedTypesOf(typeof(GenericGamePacketHandlerAsync<>)).PropertiesAutowired();
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