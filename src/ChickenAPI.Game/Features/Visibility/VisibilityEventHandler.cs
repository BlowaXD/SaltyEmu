using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Features.Portals;
using ChickenAPI.Game.Features.Visibility.Args;
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Packets.Game.Client.Shops;
using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Packets.Game.Server._NotYetSorted;

namespace ChickenAPI.Game.Features.Visibility
{
    public class VisibilityEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<VisibilityEventHandler>();

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case VisibilitySetInvisibleEventArgs invisibleEvent:
                    SetInvisible(entity, invisibleEvent);
                    break;

                case VisibilitySetVisibleEventArgs visibleEvent:
                    SetVisible(entity, visibleEvent);
                    break;
            }
        }

        private static void SetVisible(IEntity entity, VisibilitySetVisibleEventArgs args)
        {
            entity.GetComponent<VisibilityComponent>().IsVisible = true;
            if (!args.Broadcast)
            {
                return;
            }

            InPacketBase inEntity = entity.GenerateInPacket();
            PairyPacket pairy = null;

            foreach (IEntity entityy in entity.EntityManager.Entities)
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

                if (!entityy.GetComponent<VisibilityComponent>().IsVisible)
                {
                    continue;
                }

                switch (entityy.Type)
                {
                    case EntityType.Monster:
                    case EntityType.Mate:
                    case EntityType.Npc:
                    case EntityType.Player:
                        session.SendPacket(entityy.GenerateInPacket());
                        if (entityy is IPlayerEntity player)
                        {
                            player.SendPacket(inEntity);
                            player.SendPacket(pairy);
                        }

                        if (entityy.Type != EntityType.Npc || !(entityy is NpcEntity npc))
                        {
                            continue;
                        }

                        if (npc.Shop != null)
                        {
                            session.SendPacket(new ShopPacket
                            {
                                VisualType = VisualType.Npc,
                                EntityId = npc.MapNpc.Id,
                                ShopId = npc.Shop.Id,
                                MenuType = npc.Shop.MenuType,
                                ShopType = npc.Shop.ShopType,
                                Name = npc.Shop.Name
                            });
                        }

                        break;
                    case EntityType.Portal:

                        if (entityy is PortalEntity portal)
                        {
                            session.SendPacket(portal.GetComponent<PortalComponent>().GenerateGpPacket());
                        }

                        break;
                }
            }
        }

        private static void SetInvisible(IEntity entity, VisibilitySetInvisibleEventArgs args)
        {
            entity.GetComponent<VisibilityComponent>().IsVisible = false;
            if (!args.Broadcast)
            {
                return;
            }

            if (entity.EntityManager?.Entities == null)
            {
                return;
            }

            if (entity is IPlayerEntity player && player.EntityManager is IBroadcastable broadcastable)
            {
                broadcastable.Broadcast(player, player.GenerateOutPacket());
            }
        }
    }
}