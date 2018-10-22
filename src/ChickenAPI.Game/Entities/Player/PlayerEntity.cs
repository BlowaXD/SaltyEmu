using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Enums.Game.Visibility;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Inventory.Extensions;
using ChickenAPI.Game.Features.Leveling;
using ChickenAPI.Game.Features.Quicklist;
using ChickenAPI.Game.Features.Skills;
using ChickenAPI.Game.Features.Specialists;
using ChickenAPI.Game.Maps.Events;
using ChickenAPI.Game.Movements.DataObjects;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Network.BroadcastRules;
using ChickenAPI.Game.Packets.Extensions;
using ChickenAPI.Game.Permissions;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Visibility;
using ChickenAPI.Game.Visibility.Events;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Game.Server.Group;

namespace ChickenAPI.Game.Entities.Player
{
    public class PlayerEntity : EntityBase, IPlayerEntity
    {
        public static readonly IAlgorithmService Algorithm = new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;
        private static IItemInstanceService ItemInstance => new Lazy<IItemInstanceService>(() => ChickenContainer.Instance.Resolve<IItemInstanceService>()).Value;
        private static ICharacterService CharacterService => new Lazy<ICharacterService>(() => ChickenContainer.Instance.Resolve<ICharacterService>()).Value;
        private static ICharacterSkillService CharacterSkillService => new Lazy<ICharacterSkillService>(() => ChickenContainer.Instance.Resolve<ICharacterSkillService>()).Value;
        private static ICharacterQuickListService CharacterQuicklistService => new Lazy<ICharacterQuickListService>(() => ChickenContainer.Instance.Resolve<ICharacterQuickListService>()).Value;
        private static IPlayerManager PlayerManager => new Lazy<IPlayerManager>(() => ChickenContainer.Instance.Resolve<IPlayerManager>()).Value;

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
            SkillComponent = new SkillComponent(this, skills);
            Components = new Dictionary<Type, IComponent>
            {
                { typeof(VisibilityComponent), _visibility },
                { typeof(MovableComponent), Movable },
                { typeof(ExperienceComponent), Experience },
                { typeof(InventoryComponent), Inventory },
                { typeof(SpecialistComponent), Sp },
                { typeof(SkillComponent), SkillComponent }
            };
        }

        public MovableComponent Movable { get; }
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
                broadcastable.Broadcast(packet, new AllExpectOne(this));
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
                broadcastable.Broadcast(packets, new AllExpectOne(this));
            }
            else
            {
                broadcastable.Broadcast(packets);
            }
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
                EmitEvent(new VisibilitySetInvisibleEventArgs { Broadcast = true, IsChangingMapLayer = true });
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
            Task.WaitAll(
                CharacterService.SaveAsync(Character),
                CharacterSkillService.SaveAsync(SkillComponent.CharacterSkills.Values),
                CharacterQuicklistService.SaveAsync(Quicklist.Quicklist),
                ItemInstance.SaveAsync(Inventory.GetItems())
            );
            Log.Info($"[SAVE] {Character.Name} saved in {(DateTime.UtcNow - before).TotalMilliseconds} ms");
        }

        #region Battle

        #region Skills

        public bool HasSkill(long skillId) => SkillComponent.Skills.ContainsKey(skillId);

        public bool CanCastSkill(long skillId) => SkillComponent.CooldownsBySkillId.Any(s => s.Item2 == skillId);
        public IDictionary<long, SkillDto> Skills { get; }

        public SkillComponent SkillComponent { get; }

        #endregion
        

        public bool IsAlive => Hp > 0;
        public bool CanAttack => true;

        public byte HpPercentage => Convert.ToByte((int)(Hp / (float)HpMax * 100));
        public byte MpPercentage => Convert.ToByte((int)(Mp / (float)MpMax * 100.0));
        public byte BasicArea { get; }
        public int Hp { get; set; }
        public int Mp { get; set; }
        public int HpMax { get; set; }
        public int MpMax { get; set; }

        #region Movements

        public bool CanMove => !Movable.IsSitting;
        public Position<short> Actual => Movable.Actual;
        public Position<short> Destination => Movable.Destination;

        public void SetPosition(Position<short> position)
        {
            Movable.Actual = position;
        }

        public void SetPosition(short x, short y)
        {
            Movable.Actual = new Position<short>(x, y);
        }

        #endregion

        #endregion

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

        #region Family

        public bool HasFamily => Family != null;
        public bool IsFamilyLeader => FamilyCharacter?.Authority == FamilyAuthority.Head;
        public FamilyDto Family { get; set; }
        public CharacterFamilyDto FamilyCharacter { get; set; }

        #endregion
    }
}