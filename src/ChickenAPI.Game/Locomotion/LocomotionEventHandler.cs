using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Locomotion.Events;
using ChickenAPI.Game.Movements.Extensions;

namespace ChickenAPI.Game.Locomotion
{
    public class LocomotionEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<LocomotionEventHandler>();

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(TransformationLocomotion),
            typeof(UnTransformationLocomotion)
        };

        public override void Execute(IEntity entity, ChickenEventArgs args)
        {
            switch (args)
            {
                case TransformationLocomotion locoevent:
                    MorphLoco(entity as IPlayerEntity, locoevent);
                    break;

                case UnTransformationLocomotion unmorphoevent:
                    UnMorphLoco(entity as IPlayerEntity, unmorphoevent);
                    break;
            }
        }

        private void MorphLoco(IPlayerEntity player, TransformationLocomotion args)
        {
            if (player.IsSitting)
            {
                player.Movable.IsSitting = false;
            }

            ItemDto item = args.Item.Item;

            player.Speed = item.Speed;
            player.LocomotionSpeed = item.Speed;
            player.Locomotion.IsVehicled = true;
            player.MorphId = (short)(item.Morph + (byte)player.Character.Gender);
            player.Broadcast(player.GenerateEffectPacket(196));
            player.Broadcast(player.GenerateCModePacket());
            player.Broadcast(player.GenerateCondPacket());
        }

        private void UnMorphLoco(IPlayerEntity player, UnTransformationLocomotion args)
        {
            player.Locomotion.IsVehicled = false;

            player.Speed = (byte)Algorithm.GetSpeed(player.Character.Class, player.Level);
            if (player.IsTransformedSp)
            {
                if (player.Sp != null)
                {
                    player.MorphId = player.Sp.Design;
                }
            }
            else
            {
                player.MorphId = 0;
            }
            player.Broadcast(player.GenerateCModePacket());
            player.Broadcast(player.GenerateCondPacket());
        }

        private static IAlgorithmService Algorithm => new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;
    }
}