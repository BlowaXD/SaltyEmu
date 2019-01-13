﻿using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Packets.Game.Client.Specialists;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class SpPacketExtension
    {
        private static readonly IGameConfiguration Configuration = new Lazy<IGameConfiguration>(ChickenContainer.Instance.Resolve<IGameConfiguration>).Value;

        public static SpPacket GenerateSpPacket(this IPlayerEntity player) => new SpPacket
        {
            AdditionalPoints = player.Character.SpAdditionPoint,
            MaxAdditionalPoints = Configuration.SpMaxAdditionalPoints,
            Points = player.Character.SpPoint,
            MaxDailyPoints = Configuration.SpMaxDailyPoints
        };
    }
}