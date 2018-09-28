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
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Visibility.Events;
using ChickenAPI.Packets.Game.Client.Shops;
using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Packets.Game.Server._NotYetSorted;

namespace ChickenAPI.Game.Visibility
{
    public class VisibilityEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<VisibilityEventHandler>();

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

        private static void SetVisible(IEntity entity, VisibilitySetVisibleEventArgs args)
        {
            if (!args.Broadcast)
            {
                return;
            }

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
                        }

                        if (entityy.Type != VisualType.Npc || !(entityy is NpcEntity npc))
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
                    case VisualType.Portal:

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
            if (!args.Broadcast)
            {
                return;
            }

            if (entity is IPlayerEntity player && player.CurrentMap is IMapLayer broadcastable)
            {
                broadcastable.Broadcast(player, player.GenerateOutPacket());
            }
        }
    }
}