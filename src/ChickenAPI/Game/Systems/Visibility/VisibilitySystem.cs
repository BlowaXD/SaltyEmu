using System;
using System.Linq.Expressions;
using ChickenAPI.ECS.Entities;
using ChickenAPI.ECS.Systems;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server;
using ChickenAPI.Utils;

namespace ChickenAPI.Game.Systems.Visibility
{
    public class VisibilitySystem : NotifiableSystemBase
    {
        private static readonly Logger Log = Logger.GetLogger<VisibilitySystem>();

        public VisibilitySystem(IEntityManager entityManager) : base(entityManager)
        {
        }

        protected override Expression<Func<IEntity, bool>> Filter =>
            entity => entity.Type == EntityType.Player || entity.Type == EntityType.Monster || entity.Type == EntityType.Mate || entity.Type == EntityType.Npc;

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
                default:
                    throw new NotImplementedException();
            }
        }

        private void SetVisible(IEntity entity, VisibilitySetVisibleEventArgs args)
        {
            Log.Info($"[ENTITY:{entity.Id}] VISIBLE");
            entity.GetComponent<VisibilityComponent>().IsVisible = true;
            if (!args.Broadcast)
            {
                return;
            }

            var inEntity = new InPacketBase(entity);

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
                        var inpacket = new InPacketBase(entityy);
                        session.SendPacket(inpacket);
                        if (entityy is IPlayerEntity player)
                        {
                            player.SendPacket(inEntity);
                        }

                        break;
                    case EntityType.Portal:
                        session.SendPacket(new GpPacket(entityy));
                        break;
                }
            }
        }

        private void SetInvisible(IEntity entity, VisibilitySetInvisibleEventArgs args)
        {
            Log.Info($"[ENTITY:{entity.Id}] INVISIBLE");
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