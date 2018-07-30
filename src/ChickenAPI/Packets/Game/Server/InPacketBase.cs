using System;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Packets.Game.Server
{
    /*
        $"in 
        1
        {Name}
        - 
        {CharacterId}
        {PositionX}
        {PositionY}
        {DirectionType}
        {(Undercover ? (byte)AuthorityType.User : Authority < AuthorityType.GameMaster ? 0 : 2)}
        {(byte)Gender}
        {(byte)HairStyle}
        {color}
        {(byte)Class}
        {GenerateEqListForPacket()}
        {Math.Ceiling(Hp / HpLoad() * 100)}
        {Math.Ceiling(Mp / MpLoad() * 100)}
        {(IsSitting ? 1 : 0)}
        {(Group?.GroupType == GroupType.Group ? (long)Group?.GroupId : -1)}
        {(fairy != null ? 4 : 0)}
        {fairy?.Item.Element ?? 0}
        0 // ???
        {fairy?.Item.Morph ?? 0}
        0 // ??
        {(UseSp || IsVehicled ? Morph : 0)}
        {GenerateEqRareUpgradeForPacket()}
        {(foe ? -1 : Family?.FamilyId ?? -1)}
        {(foe ? name : Family?.Name ?? "-")}
        {(GetDignityIco() == 1 ? GetReputIco() : -GetDignityIco())}
        {(Invisible ? 1 : 0)}
        {(UseSp ? MorphUpgrade : 0)}
        {faction}
        {(UseSp ? MorphUpgrade2 : 0)}
        {Level}
        {Family?.FamilyLevel ?? 0}
        {ArenaWinner}
        {(Authority == AuthorityType.Moderator ? 500 : Compliment)}
        {Size} 
        {HeroLevel}";
    */
    [PacketHeader("in")]
    public class InPacketBase : PacketBase
    {
        public InPacketBase(IEntity entity)
        {
            switch (entity.Type)
            {
                case EntityType.Monster:
                    FillMonster(entity);
                    break;
                case EntityType.Player:
                    FillPlayer((IPlayerEntity)entity);
                    break;
                case EntityType.Mate:
                    FillMate(entity);
                    break;
                case EntityType.Npc:
                    FillNpc(entity);
                    break;
                case EntityType.Portal:
                    break;
                case EntityType.Effect:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FillNpc(IEntity entity)
        {
            var npcMonster = entity.GetComponent<NpcMonsterComponent>();
            var movable = entity.GetComponent<MovableComponent>();
            var battle = entity.GetComponent<BattleComponent>();


            VisualType = VisualType.Npc;
            Name = npcMonster.Vnum.ToString();
            Unknown = npcMonster.MapNpcMonsterId.ToString();
            PositionX = movable.Actual.X;
            PositionY = movable.Actual.Y;
            DirectionType = movable.DirectionType;
            InNpcSubPacket = new InNpcSubPacket
            {
                HpPercentage = 100,
                MpPercentage = 100,
                Dialog = 0,
                Unknown1 = 0,
                Unknown2 = 0,
                Unknown3 = -1,
                Unknown4 = 1,
                IsSitting = false,
                Unknown5 = -1,
                Unknown6 = "-",
                Unknown7 = 0,
                Unknown8 = -1,
                Unknown9 = 0,
                Unknown10 = 0,
                Unknown11 = 0,
                Unknown12 = 0,
                Unknown13 = 0,
                Unknown14 = 0,
                Unknown15 = 0,
                Unknown16 = 0,
            };
            //in 2 {(int)(CurrentHp / (float)Npc.MaxHP * 100)} {(int)(CurrentMp / (float)Npc.MaxMP * 100)} {Dialog} 0 0 -1 1 {(IsSitting ? 1 : 0)} -1 - 0 -1 0 0 0 0 0 0 0 0"

        }

        private void FillMate(IEntity entity)
        {
            VisualType = VisualType.Npc;
        }

        private void FillPlayer(IPlayerEntity entity)
        {
            var character = entity.GetComponent<CharacterComponent>();
            var battle = entity.GetComponent<BattleComponent>();
            var movable = entity.GetComponent<MovableComponent>();

            VisualType = VisualType.Character;
            Name = entity.GetComponent<NameComponent>().Name;
            Unknown = "-";
            VNum = character.Id;
            PositionX = movable.Actual.X;
            PositionY = movable.Actual.Y;
            DirectionType = movable.DirectionType;
            InCharacterSubPacket = new InCharacterSubPacketBase
            {
                Authority = entity.Session.Account.Authority > AuthorityType.GameMaster ? (byte)2 : (byte)0,
                Gender = character.Gender,
                HairStyle = character.HairStyle,
                HairColor = character.HairColor,
                Class = character.Class,
                Equipment = new InventoryWearSubPacket(entity.Inventory),
                HpPercentage = battle.HpPercentage,
                MpPercentage = battle.MpPercentage,
                IsSitting = false,
                GroupId = -1,
                FairyId = 0,
                FairyElement = 0,
                IsBoostedFairy = 0,
                FairyMorph = 0,
                EntryType = 0,
                Morph = 0,
                EquipmentRare = "00",
                EquipmentRareTwo = "00",
                FamilyId = -1,
                FamilyName = "-", // if not put -1
                ReputationIcon = 27,
                Invisible = !entity.GetComponent<VisibilityComponent>().IsVisible,
                SpUpgrade = 0,
                Faction = FactionType.Neutral, // todo faction system
                SpDesign = 0,
                Level = entity.Experience.Level,
                FamilyLevel = 0,
                ArenaWinner = character.ArenaWinner,
                Compliment = character.Compliment,
                Size = 10,
                HeroLevel = entity.Experience.HeroLevel
            };
        }

        private void FillMonster(IEntity entity)
        {
            var battle = entity.GetComponent<BattleComponent>();
            var npcMonster = entity.GetComponent<NpcMonsterComponent>();
            var movable = entity.GetComponent<MovableComponent>();

            VisualType = VisualType.Monster;
            Name = npcMonster.Vnum.ToString();
            Unknown = npcMonster.MapNpcMonsterId.ToString();
            PositionX = movable.Actual.X;
            PositionY = movable.Actual.Y;
            DirectionType = movable.DirectionType;
            InMonsterSubPacket = new InMonsterSubPacket
            {
                HpPercentage = Convert.ToByte(Math.Ceiling(battle.Hp / (battle.HpMax * 100.0))),
                MpPercentage = Convert.ToByte(Math.Ceiling(battle.Mp / (battle.MpMax * 100.0))),
                Unknown1 = 0,
                Unknown2 = 0,
                Unknown3 = -1,
                NoAggressiveIcon = !npcMonster.IsAggressive,
                Unknown4 = 0,
                Unknown5 = -1,
                Unknown6 = "-",
                Unknown7 = 0,
                Unknown8 = -1,
                Unknown9 = 0,
                Unknown10 = 0,
                Unknown11 = 0,
                Unknown12 = 0,
                Unknown13 = 0,
                Unknown14 = 0,
                Unknown15 = 0,
                Unknown16 = 0
            };
        }

        #region Properties

        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1, IsOptional = true)]
        public string Name { get; set; }

        [PacketIndex(2)]
        public string Unknown { get; set; }

        [PacketIndex(3, IsOptional = true)]
        public long? VNum { get; set; }

        [PacketIndex(4)]
        public short PositionX { get; set; }

        [PacketIndex(5)]
        public short PositionY { get; set; }

        [PacketIndex(6, IsOptional = true)]
        public DirectionType DirectionType { get; set; }

        [PacketIndex(7, IsOptional = true)]
        public short? Amount { get; set; }

        [PacketIndex(8, IsOptional = true, RemoveSeparator = true)]
        public InMonsterSubPacket InMonsterSubPacket { get; set; }

        [PacketIndex(9, IsOptional = true, RemoveSeparator = true)]
        public InCharacterSubPacketBase InCharacterSubPacket { get; set; }

        [PacketIndex(10, IsOptional = true, RemoveSeparator = true)]
        public InNpcSubPacket InNpcSubPacket { get; set; }

        #endregion
    }
}