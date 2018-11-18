using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player
{
    public class PlayerEventHandler : EventHandlerBase
    {
        private static IAlgorithmService Algorithm => new Lazy<IAlgorithmService>(() => ChickenContainer.Instance.Resolve<IAlgorithmService>()).Value;

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(ExperienceGainEvent),
            typeof(ReputationGainEvent),
            typeof(PlayerDeathEvent),
            typeof(PlayerLevelUpEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case ExperienceGainEvent expGain:
                    if (!(entity is IPlayerEntity player))
                    {
                        return;
                    }
                    player.AddExperience(expGain.Experience);
                    player.AddJobExperience(expGain.JobExperience);
                    player.AddHeroExperience(expGain.HeroExperience);
                    break;

                case ReputationGainEvent repGain:
                    if (!(entity is IPlayerEntity session))
                    {
                        return;
                    }

                    break;

                case PlayerDeathEvent death:
                    if (!(entity is IPlayerEntity pl))
                    {
                        return;
                    }

                    // If PvP generate reward
                    // Clear Buff/Debuff
                    // Send message box
                    // Respawn with location according to the choice made in the message box
                    break;

                case PlayerLevelUpEvent levelUp:
                    if (!(entity is IPlayerEntity play))
                    {
                        return;
                    }

                    play.HpMax = Algorithm.GetHpMax(play.Character.Class, play.Level);
                    play.Hp = play.HpMax;
                    play.MpMax = Algorithm.GetMpMax(play.Character.Class, play.Level);
                    play.Mp = play.MpMax;
                    play.SendPacket(play.GenerateStatPacket());
                    play.SendPacket(new LevelUpPacket {CharacterId = play.Id});
                    switch (levelUp.LevelUpType)
                    {
                        case LevelUpType.Level:
                            play.GenerateEffectPacket(6);
                            break;
                        case LevelUpType.HeroLevel:
                        case LevelUpType.JobLevel:
                            play.GenerateEffectPacket(8);
                            break;
                    }
                    play.GenerateEffectPacket(198);
                    break;
            }
        }
    }
}