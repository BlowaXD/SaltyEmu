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
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Buffs;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Groups;
using ChickenAPI.Game.Inventory;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Locomotion.DataObjects;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps.Events;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Quicklist;
using ChickenAPI.Game.Relations;
using ChickenAPI.Game.Shops;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game.Visibility.Events;
using ChickenAPI.Game._BroadcastRules;
using ChickenAPI.Game._ECS.Components;
using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public class PlayerEntity : EntityBase, IPlayerEntity
    {
        private const int NcifDelay = 400;
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
            Relations = new RelationList();
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
            Locomotion = new LocomotionComponent(this);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), _visibility },
                { typeof(InventoryComponent), Inventory },
                { typeof(SkillComponent), SkillComponent },
                { typeof(LocomotionComponent), Locomotion }
            };
            Mates = new List<IMateEntity>();

            #region Stat

            Defence = (short)Algorithm.GetDefenceClose(Character.Class, Level);
            DefenceDodge = (short)Algorithm.GetDodgeClose(Character.Class, Level);
            DistanceDefence = (short)Algorithm.GetDefenceRange(Character.Class, Level);
            DistanceDefenceDodge = (short)Algorithm.GetDodgeRanged(Character.Class, Level);
            MagicalDefence = (short)Algorithm.GetDefenceMagic(Character.Class, Level);
            MinHit = (short)Algorithm.GetMinHit(Character.Class, Level);
            MaxHit = (short)Algorithm.GetMaxHit(Character.Class, Level);
            HitRate = (byte)Algorithm.GetHitRate(Character.Class, Level);
            CriticalChance = Algorithm.GetHitCritical(Character.Class, Level);
            CriticalRate = (byte)Algorithm.GetHitCriticalRate(Character.Class, Level);
            DistanceCriticalChance = Algorithm.GetDistCritical(Character.Class, Level);
            DistanceCriticalRate = Algorithm.GetDistCriticalRate(Character.Class, Level);

            #endregion Stat
        }

        private MovableComponent Movable { get; }

        public long Gold
        {
            get => Character.Gold;
            set => Character.Gold = value;
        }

        public InventoryComponent Inventory { get; }
        public LocomotionComponent Locomotion { get; }
        public IRelationList Relations { get; }
        public CharacterDto Character { get; }
        public List<IMateEntity> Mates { get; set; }
        public IEnumerable<IMateEntity> ActualMates => Mates.Where(s => s.Mate.IsTeamMember).OrderByDescending(s => s.Mate.MateType);

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
            EmitEvent(new VisibilitySetVisibleEvent
            {
                Broadcast = true,
                IsChangingMapLayer = true
            });
        }

        public Task SendPacketAsync<T>(T packet) where T : IPacket => Session.SendPacketAsync(packet);

        public Task SendPacketsAsync<T>(IEnumerable<T> packets) where T : IPacket => Session.SendPacketsAsync(packets);

        public Task SendPacketsAsync(IEnumerable<IPacket> packets) => Session.SendPacketsAsync(packets);

        public override void Dispose()
        {
            EmitEvent(new MapLeaveEvent { Map = CurrentMap });
            Group?.Players.Remove(this);
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

        public double LastPortal { get; set; }
        public ItemInstanceDto Fairy => Inventory.GetWeared(EquipmentType.Fairy);
        public ItemInstanceDto Weapon => Inventory.GetWeared(EquipmentType.MainWeapon);
        public ItemInstanceDto SecondaryWeapon => Inventory.GetWeared(EquipmentType.SecondaryWeapon);
        public ItemInstanceDto Armor => Inventory.GetWeared(EquipmentType.Armor);

        public bool IsTransformedLocomotion => Locomotion.IsVehicled;

        public DateTime DateLastPortal { get; set; }

        public bool HasShop => Shop != null;
        public PersonalShop Shop { get; set; }

        #region stat

        public int MinHit { get; set; }

        public int MaxHit { get; set; }

        public int HitRate { get; set; }
        public int CriticalChance { get; set; }
        public short CriticalRate { get; set; }
        public int DistanceCriticalChance { get; set; }
        public int DistanceCriticalRate { get; set; }
        public short WaterResistance { get; set; }
        public short FireResistance { get; set; }
        public short LightResistance { get; set; }
        public short DarkResistance { get; set; }

        public short Defence { get; set; }

        public short DefenceDodge { get; set; }

        public short DistanceDefence { get; set; }

        public short DistanceDefenceDodge { get; set; }

        public short MagicalDefence { get; set; }

        #endregion stat

        #region Broadcast

        public Task BroadcastAsync<T>(T packet) where T : IPacket => BroadcastAsync(packet, null);

        public Task BroadcastAsync<T>(T packet, IBroadcastRule rule) where T : IPacket => CurrentMap?.BroadcastAsync(packet, rule);

        public Task BroadcastAsync<T>(IEnumerable<T> packets) where T : IPacket => BroadcastAsync(packets, null);

        public Task BroadcastAsync<T>(IEnumerable<T> packets, IBroadcastRule rule) where T : IPacket => CurrentMap?.BroadcastAsync(packets, rule);

        public Task BroadcastAsync(IEnumerable<IPacket> packets) => BroadcastAsync(packets, null);

        public Task BroadcastAsync(IEnumerable<IPacket> packets, IBroadcastRule rule) => CurrentMap?.BroadcastAsync(packets, rule);

        public Task BroadcastExceptSenderAsync<T>(T packet) where T : IPacket => BroadcastAsync(packet, new AllExpectOne(this));

        public Task BroadcastExceptSenderAsync<T>(IEnumerable<T> packets) where T : IPacket => BroadcastAsync(packets, new AllExpectOne(this));

        #endregion Broadcast

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
        private readonly List<BuffContainer> _buffs = new List<BuffContainer>();
        public ICollection<BuffContainer> Buffs => _buffs;

        #region Target

        private IBattleEntity _target;
        public bool HasTarget => Target != null;

        public IBattleEntity Target
        {
            get
            {
                if (LastTarget.AddMilliseconds(NcifDelay) <= DateTime.Now)
                {
                    _target = null;
                }

                return _target;
            }
            set
            {
                _target = value;
                LastTarget = DateTime.Now;
            }
        }

        public DateTime LastTarget { get; private set; }

        #endregion Target

        public DateTime LastTimeKilled { get; set; }
        public DateTime LastHitReceived { get; set; }

        #endregion Stats

        #region Movements

        public DirectionType DirectionType => Movable.DirectionType;

        public bool IsSitting
        {
            get => Movable.IsSitting;
            set => Movable.IsSitting = value;
        }

        public bool IsWalking => !Movable.IsSitting;
        public bool CanMove => !Movable.IsSitting;
        public bool IsStanding => !Movable.IsSitting;

        public byte Speed
        {
            get => Movable.Speed;
            set => Movable.Speed = value;
        }

        public byte LocomotionSpeed
        {
            get => Locomotion.Speed;
            set => Locomotion.Speed = value;
        }

        public DateTime LastMove { get; set; }

        // todo manage Position of player in instanciated mapLayers
        public Position<short> Position
        {
            get => Movable.Actual;
            set => Movable.Actual = value;
        }

        public Position<short> Destination => Movable.Destination;

        #endregion Movements

        #endregion Battle

        #region Visibility

        public bool IsVisible => _visibility.IsVisible;

        public bool IsInvisible => _visibility.IsInvisible;

        VisibilityType IVisibleCapacity.Visibility
        {
            get => _visibility.Visibility;
            set => _visibility.Visibility = value;
        }

        public byte Size { get; set; }

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
        ///     Find a better way to manage it
        /// </summary>
        public short MorphId { get; set; }

        public DateTime LastMorphUtc { get; set; }

        public bool HasSpWeared => Sp != null;
        public bool IsTransformedSp => HasSpWeared && MorphId == Sp.Item.Morph;

        public ItemInstanceDto Sp => Inventory.Wear[(int)EquipmentType.Sp];

        public int SpCoolDown { get; set; }

        #endregion Specialist

        #region Group

        public Group Group { get; set; }
        public bool HasGroup => Group != null;
        public bool IsGroupLeader => HasGroup && Group.Leader == this;
        public int GroupMembersCount => HasGroup ? Group.Players.Count : 0;

        #endregion Group
    }
}