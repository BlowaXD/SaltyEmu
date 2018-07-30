using System;
using Autofac;
using ChickenAPI.Data.AccessLayer.Character;
using ChickenAPI.Data.TransferObjects.Character;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Utils;

namespace ChickenAPI.Game.Components
{
    public class BattleComponent : IComponent
    {
        public BattleComponent(IEntity entity)
        {
            Entity = entity;
        }

        public BattleComponent(IEntity entity, CharacterDto dto) : this(entity)
        {
            var algo = Container.Instance.Resolve<IAlgorithmService>();
            HpMax = algo.GetHpMax(dto.Class, dto.Level);
            Hp = HpMax;
            MpMax = algo.GetMpMax(dto.Class, dto.Level);
            Mp = MpMax;
        }

        public byte HpPercentage => Convert.ToByte(Math.Ceiling(Hp / (HpMax * 100.0)));
        public byte MpPercentage => Convert.ToByte(Math.Ceiling(Mp / (MpMax * 100.0)));

        public int Hp { get; set; }

        public int HpMax { get; set; }

        public int Mp { get; set; }

        public int MpMax { get; set; }

        public short Morph { get; set; }

        public bool CanAttack { get; set; } = false;

        public bool CanMove { get; set; } = false;

        public IEntity Entity { get; }
    }
}