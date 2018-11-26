using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps.Events;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Network.BroadcastRules;
using ChickenAPI.Game.Quicklist;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Specialists;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game.Visibility.Events;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public class PlayerEntity : EntityBase, IPlayerEntity
    {
        private static readonly IAlgorithmService Algorithm = new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;
        private static readonly IItemInstanceService ItemInstance = new Lazy<IItemInstanceService>(() => ChickenContainer.Instance.Resolve<IItemInstanceService>()).Value;
        private static readonly ICharacterService CharacterService = new Lazy<ICharacterService>(() => ChickenContainer.Instance.Resolve<ICharacterService>()).Value;
        private static readonly ICharacterSkillService CharacterSkillService = new Lazy<ICharacterSkillService>(() => ChickenContainer.Instance.Resolve<ICharacterSkillService>()).Value;

        private static readonly ICharacterQuickListService
            CharacterQuicklistService = new Lazy<ICharacterQuickListService>(() => ChickenContainer.Instance.Resolve<ICharacterQuickListService>()).Value;

        private static readonly IPlayerManager PlayerManager = new Lazy<IPlayerManager>(() => ChickenContainer.Instance.Resolve<IPlayerManager>()).Value;

        public PlayerEntity(ISession session, CharacterDto dto, IEnumerable<CharacterSkillDto> skills, IEnumerable<CharacterQuicklistDto> quicklist) : base(VisualType.Character, dto.Id)
        {
            Session = session;
            Character = dto;
            Quicklist = new QuicklistComponent(this, quicklist);

            HpMax = Algorithm.GetHpMax(dto.Class, dto.Level);
            Hp = HpMax;
            MpMax = Algorithm.GetMpMax(dto.Class, dto.Level);
            Mp = MpMax;
            BasicArea = 1;
            Inventory = new InventoryComponent(this);
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
            SkillComponent = new SkillComponent(this, skills);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), _visibility },
                { typeof(MovableComponent), Movable },
                { typeof(InventoryComponent), Inventory },
                { typeof(SkillComponent), SkillComponent }
            };
        }

        public MovableComponent Movable { get; }
        public InventoryComponent Inventory { get; }
        public CharacterDto Character { get; }

        public CharacterNameAppearance NameAppearance
        {
            get
            {
                if (IsInvisible)
                {
                    return CharacterNameAppearance.Invisible;
                }

                if (Session.Account.Authority >= AuthorityType.GameMaster)
                {
                    return CharacterNameAppearance.GameMaster;
                }

                return CharacterNameAppearance.Player;
            }
        }

        public QuicklistComponent Quicklist { get; }

        public ISession Session { get; }

        public long LastPulse { get; }

        public void Broadcast<T>(T packet) where T : IPacket
        {
            Broadcast(packet, null);
        }

        public void Broadcast<T>(IEnumerable<T> packets) where T : IPacket
        {
            Broadcast(packets, null);
        }

        public void BroadcastExceptSender<T>(T packet) where T : IPacket
        {
            Broadcast(packet, new AllExpectOne(this));
        }

        public void BroadcastExceptSender<T>(IEnumerable<T> packets) where T : IPacket
        {
            Broadcast(packets, new AllExpectOne(this));
        }

        public void Broadcast<T>(T packet, IBroadcastRule rule) where T : IPacket
        {
            CurrentMap?.Broadcast(packet, rule);
        }

        public void Broadcast<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket
        {
            CurrentMap?.Broadcast(packets, rule);
        }

        public void Broadcast(IEnumerable<IPacket> packets)
        {
            Broadcast(packets, null);
        }

        public void Broadcast(IEnumerable<IPacket> packets, IBroadcastRule rule)
        {
            CurrentMap?.Broadcast(packets, rule);
        }

        public override void TransferEntity(IMapLayer map)
        {
            if (CurrentMap == map)
            {
                return;
            }

            if (CurrentMap != null)
            {
                EmitEvent(new MapLeaveEvent { Map = CurrentMap });
            }

            base.TransferEntity(map);

            EmitEvent(new MapJoinEvent { Map = map });
            EmitEvent(new VisibilitySetVisibleEventArgs
            {
                Broadcast = true,
                IsChangingMapLayer = true,
            });
        }

        public void SendPacket<T>(T packetBase) where T : IPacket => Session.SendPacket(packetBase);

        public void SendPackets<T>(IEnumerable<T> packets) where T : IPacket
        {
            if (packets == null)
            {
                return;
            }

            foreach (T i in packets)
            {
                Session.SendPacket(i);
            }
        }

        public void SendPackets(IEnumerable<IPacket> packets) => Session.SendPackets(packets);

        public override void Dispose()
        {
            PlayerManager.UnregisterPlayer(this);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            DateTime before = DateTime.UtcNow;
            Character.MapX = Position.X;
            Character.MapY = Position.Y;
            Character.MapId = (short)CurrentMap.Map.Id;
            CharacterService.Save(Character);
            CharacterSkillService.Save(SkillComponent.CharacterSkills.Values);
            CharacterQuicklistService.Save(Quicklist.Quicklist);
            ItemInstance.Save(Inventory.GetItems());
            Log.Info($"[SAVE] {Character.Name} saved in {(DateTime.UtcNow - before).TotalMilliseconds} ms");
        }

        #region Battle

        #region Skills

        public bool HasSkill(long skillId) => SkillComponent.Skills.ContainsKey(skillId);

        public bool CanCastSkill(long skillId) => SkillComponent.CooldownsBySkillId.Any(s => s.Item2 == skillId);

        public IDictionary<long, SkillDto> Skills { get; }

        public SkillComponent SkillComponent { get; }

        #endregion Skills

        #region Stats

        public bool IsAlive => Hp > 0;
        public bool CanAttack => true;

        public byte HpPercentage => Convert.ToByte((int)(Hp / (float)HpMax * 100));
        public byte MpPercentage => Convert.ToByte((int)(Mp / (float)MpMax * 100.0));
        public byte BasicArea { get; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int HpMax { get; set; }
        public int MpMax { get; set; }
        public DateTime LastTimeKilled { get; set; }

        #endregion Stats

        #region Movements

        // todo manage Position of player in instanciated mapLayers
        public Position<short> Position { get; }

        public bool IsSitting => Movable.IsSitting;
        public bool IsWalking => !Movable.IsSitting;
        public bool CanMove => !Movable.IsSitting;
        public bool IsStanding => !Movable.IsSitting;

        public byte Speed
        {
            get => Movable.Speed;
            set => Movable.Speed = value;
        }

        public DateTime LastMove { get; }
        public Position<short> Actual => Movable.Actual;
        public Position<short> Destination => Movable.Destination;

        #endregion Movements

        #endregion Battle

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

        #endregion Visibility

        #region Family

        public bool HasFamily => Family != null;
        public bool IsFamilyLeader => FamilyCharacter?.Authority == FamilyAuthority.Head;
        public FamilyDto Family { get; set; }
        public CharacterFamilyDto FamilyCharacter { get; set; }

        #endregion Family

        #region Experience

        public byte Level
        {
            get => Character.Level;
            set => Character.Level = value;
        }

        public long LevelXp
        {
            get => Character.LevelXp;
            set => Character.LevelXp = value;
        }

        public byte HeroLevel
        {
            get => Character.HeroLevel;
            set => Character.HeroLevel = value;
        }

        public long HeroLevelXp
        {
            get => Character.HeroXp;
            set => Character.HeroXp = value;
        }

        public byte JobLevel
        {
            get => IsTransformedSp ? Sp.Level : Character.JobLevel;
            set => Character.JobLevel = value;
        }

        public long JobLevelXp
        {
            get => Character.JobLevelXp;
            set => Character.JobLevelXp = value;
        }

        #endregion Experience

        #region Specialist

        /// <summary>
        /// Find a better way to manage it
        /// </summary>
        public short MorphId { get; set; }

        public DateTime LastMorphUtc { get; set; }

        public bool HasSpWeared => Sp != null;
        public bool IsTransformedSp => HasSpWeared && MorphId == Sp.Item.Morph;

        public ItemInstanceDto Sp => Inventory.Wear[(int)EquipmentType.Sp];

        #endregion Specialist

        public long Gold
        {
            get => Character.Gold;
            set => Character.Gold = value;
        }

        public double LastPortal { get; set; }

        public DateTime DateLastPortal { get; set; }

        #region Group

        public GroupDto Group { get; set; }
        public bool HasGroup => Group != null;
        public bool IsGroupLeader => HasGroup && Group.Leader == this;

        #endregion
    }
}