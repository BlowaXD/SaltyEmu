﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Battle.DataObjects;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Features.Families;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Inventory.Extensions;
using ChickenAPI.Game.Features.Leveling;
using ChickenAPI.Game.Features.Quicklist;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Specialists;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Permissions;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game.Visibility.Events;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Server.Group;

namespace ChickenAPI.Game.Entities.Player
{
    public class PlayerEntity : EntityBase, IPlayerEntity
    {
        private static IItemInstanceService ItemInstance => new Lazy<IItemInstanceService>(() => ChickenContainer.Instance.Resolve<IItemInstanceService>()).Value;
        private static ICharacterService CharacterService => new Lazy<ICharacterService>(() => ChickenContainer.Instance.Resolve<ICharacterService>()).Value;
        private static ICharacterSkillService CharacterSkillService => new Lazy<ICharacterSkillService>(() => ChickenContainer.Instance.Resolve<ICharacterSkillService>()).Value;
        private static ICharacterQuickListService CharacterQuicklistService => new Lazy<ICharacterQuickListService>(() => ChickenContainer.Instance.Resolve<ICharacterQuickListService>()).Value;

        public PlayerEntity(ISession session, CharacterDto dto, IEnumerable<CharacterSkillDto> skills, IEnumerable<CharacterQuicklistDto> quicklist) : base(VisualType.Character, dto.Id)
        {
            Session = session;
            Character = dto;
            Quicklist = new QuicklistComponent(this, quicklist);
            Battle = new BattleComponent(this, dto);
            Inventory = new InventoryComponent(this);
            Experience = new ExperienceComponent(this)
            {
                Level = dto.Level,
                LevelXp = dto.LevelXp,
                JobLevel = dto.JobLevel,
                JobLevelXp = dto.JobLevelXp,
                HeroLevel = dto.HeroLevel,
                HeroLevelXp = dto.HeroXp
            };
            Movable = new MovableComponent(this)
            {
                Actual = new Position<short>
                {
                    X = dto.MapX,
                    Y = dto.MapY
                },
                Destination = new Position<short>
                {
                    X = dto.MapX,
                    Y = dto.MapY
                }
            };
            _visibility = new VisibilityComponent(this);
            Skills = new SkillComponent(this, skills);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), _visibility },
                { typeof(MovableComponent), Movable },
                { typeof(BattleComponent), Battle },
                { typeof(ExperienceComponent), Experience },
                { typeof(FamilyComponent), new FamilyComponent(this) },
                { typeof(InventoryComponent), Inventory },
                { typeof(SpecialistComponent), Sp },
                { typeof(SkillComponent), Skills }
            };
        }


        public SkillComponent Skills { get; }
        public MovableComponent Movable { get; }
        public BattleComponent Battle { get; }
        public InventoryComponent Inventory { get; }
        public ExperienceComponent Experience { get; }
        public CharacterDto Character { get; }
        public QuicklistComponent Quicklist { get; }
        public SpecialistComponent Sp { get; }
        public ISession Session { get; }

        public bool HasPermission(PermissionType permission)
        {
            return Session.Account.PermissibleRank.HasPermission(permission);
        }

        public bool HasPermission(string permissionKey)
        {
            return Session.Account.PermissibleRank.HasPermission(permissionKey);
        }

        public bool HasPermission(PermissionsRequirementsAttribute permissions)
        {
            return HasPermission(permissions.PermissionType) && HasPermission(permissions.PermissionName);
        }

        public long LastPulse { get; }

        public void Broadcast<T>(T packet) where T : IPacket
        {
            Broadcast(packet, false);
        }

        public void Broadcast<T>(IEnumerable<T> packets) where T : IPacket
        {
            Broadcast(packets, false);
        }

        public void Broadcast<T>(T packet, bool doNotReceive) where T : IPacket
        {
            if (!(CurrentMap is IMapLayer broadcastable))
            {
                return;
            }

            if (doNotReceive)
            {
                broadcastable.Broadcast(this, packet);
            }
            else
            {
                broadcastable.Broadcast(packet);
            }
        }

        public void Broadcast<T>(IEnumerable<T> packets, bool doNotReceive) where T : IPacket
        {
            if (!(CurrentMap is IMapLayer broadcastable))
            {
                return;
            }

            if (doNotReceive)
            {
                broadcastable.Broadcast(this, packets);
            }
            else
            {
                broadcastable.Broadcast(packets);
            }
        }

        public override void TransferEntity(IMapLayer manager)
        {
            if (CurrentMap != null)
            {
                NotifyEventHandler<VisibilityEventHandler>(new VisibilitySetInvisibleEventArgs { Broadcast = true, IsChangingMapLayer = true });
            }

            base.TransferEntity(manager);

            if (!(manager is IMapLayer map))
            {
                return;
            }

            SendPacket(this.GenerateCInfoPacket());
            SendPacket(this.GenerateCModePacket());
            SendPacket(this.GenerateEqPacket());
            SendPacket(this.GenerateEquipmentPacket());
            SendPacket(this.GenerateLevPacket());
            SendPacket(this.GenerateStPacket());

            SendPacket(this.GenerateAtPacket());
            SendPacket(this.GenerateCondPacket());
            SendPacket(map.Map.GenerateCMapPacket());
            SendPacket(this.GenerateStatCharPacket());
            SendPacket(this.GeneratePairyPacket());
            // Pst()
            // Act6() : Act()
            SendPacket(new PInitPacket());
            // PInitPacket
            // ScPacket
            // ScpStcPacket
            // FcPacket
            // Act4Raid ? DgPacket() : RaidMbf
            // MapDesignObjects()
            // MapDesignObjectsEffects
            // MapItems()
            // Gp()
            //SendPacket(new RsfpPacket()); // Minimap Position
            //SendPacket(new CondPacketBase(this));
            NotifyEventHandler<VisibilityEventHandler>(new VisibilitySetVisibleEventArgs
            {
                Broadcast = true,
                IsChangingMapLayer = true
            });
            SendPacket(this.GenerateInPacket());
            SendPacket(this.GenerateStatPacket());
        }

        public void SendPacket<T>(T packetBase) where T : IPacket => Session.SendPacket(packetBase);

        public void SendPackets<T>(IEnumerable<T> packets) where T : IPacket
        {
            foreach (T i in packets)
            {
                Session.SendPacket(i);
            }
        }

        public void SendPackets(IEnumerable<IPacket> packets) => Session.SendPackets(packets);

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            DateTime before = DateTime.UtcNow;
            Task.WaitAll(
                CharacterService.SaveAsync(Character),
                // CharacterSkillService.SaveAsync(Character.Skills),
                CharacterQuicklistService.SaveAsync(Quicklist.Quicklist),
                ItemInstance.SaveAsync(Inventory.GetItems())
            );
            Log.Info($"[SAVE] {Character.Name} saved in {(DateTime.UtcNow - before).TotalMilliseconds} ms");
        }

        #region Visibility

        public event EventHandlerWithoutArgs<IVisibleEntity> Invisible
        {
            add => _visibility.Invisible += value;
            remove => _visibility.Invisible -= value;
        }

        public event EventHandlerWithoutArgs<IVisibleEntity> Visible
        {
            add => _visibility.Visible += value;
            remove => _visibility.Visible -= value;
        }

        public bool IsVisible => _visibility.IsVisible;

        public bool IsInvisible => _visibility.IsInvisible;

        VisibilityType IVisibleCapacity.Visibility
        {
            get => _visibility.Visibility;
            set => _visibility.Visibility = value;
        }

        private VisibilityComponent _visibility { get; }

        #endregion
    }
}