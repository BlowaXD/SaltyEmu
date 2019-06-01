using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Enums.Game.BCard;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Battle.Extensions;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.BCards;
using ChickenAPI.Game.Buffs;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.ServerPackets.Battle;

namespace SaltyEmu.BasicPlugin.BCardHandlers
{
    public static class BCardBuffHandler
    {
        public static BfPacket GenerateBfPacket(this IPlayerEntity player, BuffContainer buff)
        {
            return new BfPacket
            {
                VisualType = player.Type,
                VisualId = player.Id,
                Buff = new BfPacket.BuffElementSubPacket
                {
                    BuffId = buff.Id,
                    ChargeValue = 0,
                    Duration = buff.Duration,
                    
                },
                BuffLevel = buff.Level,
                
            };
        }

        [BCardEffectHandler(BCardType.Buff)]
        public static void Handle(IBattleEntity target, IBattleEntity sender, BCardDto bcard)
        {
            var random = ChickenContainer.Instance.Resolve<IRandomGenerator>();
            if (random.Next() >= bcard.FirstData)
            {
                return;
            }

            CardDto card = ChickenContainer.Instance.Resolve<ICardService>().GetById(bcard.SecondData);

            if (card == null)
            {
                //Log.Debug($"Couldn't find any buff with card Id : {bcard.SecondData}");
                return;
            }

            var buff = new BuffContainer(card, sender.Level);
            target.AddBuff(buff);
            if (target is IPlayerEntity player)
            {
                player.SendPacketAsync(player.GenerateBfPacket(buff));
            }
        }
    }
}