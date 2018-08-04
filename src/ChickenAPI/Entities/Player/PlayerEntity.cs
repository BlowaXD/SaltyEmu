using System;
using System.Collections.Generic;
using ChickenAPI.Core.ECS.Components;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Game.Components;
using ChickenAPI.Game.Game.Maps;
using ChickenAPI.Game.Game.Network;
using ChickenAPI.Game.Game.Systems.Visibility;
using ChickenAPI.Game.Packets;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Packets.Game.Client;
using ChickenAPI.Game.Packets.Game.Server;
using ChickenAPI.Game.Packets.Game.Server.Group;
using ChickenAPI.Game.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Entities.Player
{
    public class PlayerEntity : EntityBase, IPlayerEntity
    {
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
            Skills = new SkillsComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), new VisibilityComponent(this) },
                { typeof(MovableComponent), Movable },
                { typeof(BattleComponent), Battle },
                { typeof(CharacterComponent), Character },
                { typeof(ExperienceComponent), Experience },
                { typeof(FamilyComponent), new FamilyComponent(this) },
                { typeof(InventoryComponent), Inventory },
                { typeof(NameComponent), Name },
                { typeof(SpecialistComponent), new SpecialistComponent(this) },
                { typeof(SkillsComponent), Skills }
            };
        }

        public SkillsComponent Skills { get; }
        public MovableComponent Movable { get; }
        public BattleComponent Battle { get; }
        public InventoryComponent Inventory { get; }
        public ExperienceComponent Experience { get; }
        public NameComponent Name { get; set; }

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

            SendPacket(this.GenerateAtPacket());

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

        public override void Dispose()
        {
            Save();
            GC.SuppressFinalize(this);
        }

        private void Save()
        {
            Log.Info($"[SAVE_START] {Session.Account.Name}");
        }
    }
}