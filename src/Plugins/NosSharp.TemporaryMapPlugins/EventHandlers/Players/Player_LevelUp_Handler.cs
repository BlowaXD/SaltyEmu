using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Events;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class Player_LevelUp_Handler : GenericEventPostProcessorBase<PlayerLevelUpEvent>
    {
        private readonly IAlgorithmService _algorithm;

        public Player_LevelUp_Handler(IAlgorithmService algorithm, ILogger log) : base(log)
        {
            _algorithm = algorithm;
        }

        protected override async Task Handle(PlayerLevelUpEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            player.HpMax = _algorithm.GetHpMax(player.Character.Class, player.Level);
            player.Hp = player.HpMax;
            player.MpMax = _algorithm.GetMpMax(player.Character.Class, player.Level);
            player.Mp = player.MpMax;
            await player.SendPacketAsync(player.GenerateLevPacket());
            await player.SendPacketAsync(player.GenerateStatPacket());
            await player.SendPacketAsync(player.GenerateLevelUpPacket());
            switch (e.LevelUpType)
            {
                case LevelUpType.Level:
                    await player.BroadcastAsync(player.GenerateEffectPacket(6));
                    break;
                case LevelUpType.HeroLevel:
                case LevelUpType.JobLevel:
                    if (e.LevelUpType == LevelUpType.JobLevel && e.Player.Character.Class == CharacterClassType.Adventurer)
                    {
                        await player.LearnAdventurerSkillsAsync();
                    }
                    await player.BroadcastAsync(player.GenerateEffectPacket(8));
                    break;
            }

            await player.BroadcastAsync(player.GenerateEffectPacket(198));
        }
    }
}