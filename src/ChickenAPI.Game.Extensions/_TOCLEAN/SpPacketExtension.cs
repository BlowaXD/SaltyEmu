using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Packets.ServerPackets.Specialists;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class SpPacketExtension
    {
        private static readonly IGameConfiguration Configuration = new Lazy<IGameConfiguration>(ChickenContainer.Instance.Resolve<IGameConfiguration>).Value;

        public static SpPacket GenerateSpPacket(this IPlayerEntity player)
        {
            if (!player.HasSpWeared)
            {
                return null;
            }

            return new SpPacket
            {
                AdditionalPoint = player.Character.SpAdditionPoint,
                MaxAdditionalPoint = (int)Configuration.SpMaxAdditionalPoints,
                SpPoint = player.Character.SpPoint,
                MaxSpPoint = (int)Configuration.SpMaxDailyPoints
            };
        }
    }
}