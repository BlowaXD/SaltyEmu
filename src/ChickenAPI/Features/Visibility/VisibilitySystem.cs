using System;
using System.Linq.Expressions;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.ECS.Systems;
using ChickenAPI.Core.ECS.Systems.Args;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Npc;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Portal;
using ChickenAPI.Game.Features.Portals;
using ChickenAPI.Game.Features.Shops.Packets;
using ChickenAPI.Game.Features.Visibility.Args;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Features.Visibility
{
    public class VisibilitySystem : NotifiableSystemBase
    {
        private static readonly Logger Log = Logger.GetLogger<VisibilitySystem>();

        public VisibilitySystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        protected override Expression<Func<IEntity, bool>> Filter =>
            entity => entity.Type == EntityType.Player || entity.Type == EntityType.Monster || entity.Type == EntityType.Mate || entity.Type == EntityType.Npc || entity.Type == EntityType.Portal;

        public override void Execute(IEntity entity, SystemEventArgs e)
        {
            if (!Match(entity))
            {
                return;
            }

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

        private void SetVisible(IEntity entity, VisibilitySetVisibleEventArgs args)
        {
            entity.GetComponent<VisibilityComponent>().IsVisible = true;
            if (!args.Broadcast)
            {
                return;
            }

            InPacketBase inEntity = entity.GenerateInPacket();

            foreach (IEntity entityy in entity.EntityManager.Entities)
            {
                if (entityy.Id == entity.Id || !Match(entityy))
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
                            session.SendPacket(new GpPacket(portal.GetComponent<PortalComponent>()));
                        }

                        break;
                }
            }
        }

        private void SetInvisible(IEntity entity, VisibilitySetInvisibleEventArgs args)
        {
            entity.GetComponent<VisibilityComponent>().IsVisible = false;
            if (!args.Broadcast)
            {
                return;
            }

            foreach (IEntity entityy in entity.EntityManager.Entities)
            {
                if (entityy.Id == entity.Id || !Match(entityy))
                {
                    continue;
                }

                if (entityy is IPlayerEntity player)
                {
                    player.SendPacket(new OutPacketBase(player));
                }
            }
        }
    }
}