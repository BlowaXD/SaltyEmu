using System;
using System.Collections.Generic;
using ChickenAPI.Data.TransferObjects.Character;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Systems.Visibility;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Client;
using ChickenAPI.Packets.Game.Server;
using ChickenAPI.Packets.Game.Server.Group;
using ChickenAPI.Packets.Game.Server.Inventory;
using ChickenAPI.Utils;

namespace ChickenAPI.Game.Entities.Player
{
    public class PlayerEntity : EntityBase, IPlayerEntity
    {
        public MovableComponent Movable { get; }
        public BattleComponent Battle { get; }
        public InventoryComponent Inventory { get; }
        public ExperienceComponent Experience { get; }
        public NameComponent Name { get; set; }

        public PlayerEntity(ISession session, CharacterDto dto) : base(EntityType.Player)
        {
            Session = session;
            Character = new CharacterComponent(this, dto);
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
            Name = new NameComponent(this)
            {
                Name = dto.Name
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
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), new VisibilityComponent(this) },
                { typeof(MovableComponent), Movable },
                { typeof(BattleComponent), Battle },
                { typeof(CharacterComponent), Character },
                {
                    typeof(ExperienceComponent), Experience
                },
                { typeof(FamilyComponent), new FamilyComponent(this) },
                { typeof(InventoryComponent), Inventory },
                {
                    typeof(NameComponent), Name
                },
                { typeof(SpecialistComponent), new SpecialistComponent(this) }
            };
        }

        public CharacterComponent Character { get; }
        public ISession Session { get; }
        public long LastPulse { get; }

        public override void TransferEntity(IEntityManager manager)
        {
            EntityManager?.NotifySystem<VisibilitySystem>(this, new VisibilitySetInvisibleEventArgs { Broadcast = true, IsChangingMapLayer = true });
            base.TransferEntity(manager);

            if (!(manager is IMapLayer map))
            {
                return;
            }

            SendPacket(new CInfoPacketBase(this));
            SendPacket(new CModePacketBase(this));
            SendPacket(new EqPacket(this));
            SendPacket(new EquipmentPacket(this));
            SendPacket(new LevPacket(this));
            SendPacket(new StPacket(this));
            SendPacket(new AtPacketBase(this));
            SendPacket(new CondPacketBase(this));
            SendPacket(new CMapPacketBase(map.Map));
            // StatChar()
            //SendPacket(new InPacketBase(this));
            // Pairy()
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
            EntityManager.NotifySystem<VisibilitySystem>(this, new VisibilitySetVisibleEventArgs
            {
                Broadcast = true,
                IsChangingMapLayer = true
            });
            SendPacket(new StatPacket(this));
        }

        public void SendPacket<T>(T packetBase) where T : IPacket => Session.SendPacket(packetBase);

        public void SendPackets(IEnumerable<IPacket> packets) => Session.SendPackets(packets);

        private void Save()
        {
            Log.Info($"[SAVE_START] {Session.Account.Name}");
        }

        public override void Dispose()
        {
            Save();
            GC.SuppressFinalize(this);
        }
    }
}