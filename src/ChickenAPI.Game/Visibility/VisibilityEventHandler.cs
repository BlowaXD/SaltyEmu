using System;
using System.Collections.Generic;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Features.Portals;
using ChickenAPI.Game.PacketHandling.Extensions;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Visibility.Events;
using ChickenAPI.Packets.Game.Client.Shops;
using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Packets.Game.Server._NotYetSorted;

namespace ChickenAPI.Game.Visibility
{
    public class VisibilityEventHandler : EventHandlerBase
    {
        private readonly Logger _log = Logger.GetLogger<VisibilityEventHandler>();

        public override ISet<Type> HandledTypes => new HashSet<Type>();

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            if (!(entity is IVisibleCapacity visible))
            {
                return;
            }

            switch (e)
            {
                case VisibilitySetInvisibleEventArgs invisibleEvent:
                    visible.Visibility = VisibilityType.Invisible;
                    SetInvisible(entity, invisibleEvent);
                    break;

                case VisibilitySetVisibleEventArgs visibleEvent:
                    visible.Visibility = VisibilityType.Visible;
                    SetVisible(entity, visibleEvent);
                    break;
            }
        }

        /// <summary>
        /// To review
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="args"></param>
        private static void SetVisible(IEntity entity, VisibilitySetVisibleEventArgs args)
        {
            if (!args.Broadcast)
            {
                return;
            }
            /*
            InPacketBase inEntity = entity.GenerateInPacket();
            PairyPacket pairy = null;

            foreach (IEntity entityy in entity.CurrentMap.Entities)
            {
                if (entityy.Id == entity.Id)
                {
                    continue;
                }

                if (!(entity is IPlayerEntity session))
                {
                    if (entityy is IPlayerEntity player)
                    {
                        player.SendPacket(inEntity);
                    }

                    continue;
                }

                if (pairy == null)
                {
                    pairy = session.GeneratePairyPacket();
                }


                if (!args.IsChangingMapLayer)
                {
                    continue;
                }

                if (entityy.GetComponent<VisibilityComponent>().IsInvisible)
                {
                    continue;
                }

                switch (entityy.Type)
                {
                    case VisualType.Character:
                    case VisualType.Npc:
                    case VisualType.Monster:
                        session.SendPacket(entityy.GenerateInPacket());
                        if (entityy is IPlayerEntity player)
                        {
                            player.SendPacket(inEntity);
                            player.SendPacket(pairy);
                            session.SendPacket(player.GeneratePairyPacket());
                        }

                        break;
                }
            }
            */
        }

        private static void SetInvisible(IEntity entity, VisibilitySetInvisibleEventArgs args)
        {
            if (!args.Broadcast)
            {
                return;
            }

            if (entity is IPlayerEntity player)
            {
                player.BroadcastExceptSender(player.GenerateOutPacket());
            }
        }
    }
}