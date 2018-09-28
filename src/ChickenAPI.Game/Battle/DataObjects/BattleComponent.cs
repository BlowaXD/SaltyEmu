﻿using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.ECS.Components;
using ChickenAPI.Game.ECS.Entities;

namespace ChickenAPI.Game.Battle.DataObjects
{
    public class BattleComponent : IComponent
    {
        public BattleComponent(IEntity entity) => Entity = entity;

        public BattleComponent(IEntity entity, CharacterDto dto) : this(entity)
        {
            var algo = ChickenContainer.Instance.Resolve<IAlgorithmService>();
            HpMax = algo.GetHpMax(dto.Class, dto.Level);
            Hp = HpMax;
            MpMax = algo.GetMpMax(dto.Class, dto.Level);
            Mp = MpMax;
            BasicArea = 1;
        }

        public byte HpPercentage => Convert.ToByte((int)(Hp / (float)HpMax * 100));
        public byte MpPercentage => Convert.ToByte((int)(Mp / (float)MpMax * 100.0));

        public byte BasicArea { get; set; }

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