using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Chat.Events;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Extensions.PacketGeneration;
using ChickenAPI.Game.Managers;

namespace SaltyEmu.BasicPlugin.EventHandlers.Chat
{
    public class Chat_GlobalChat_Handler : GenericEventPostProcessorBase<GlobalChatEvent>
    {
        private readonly IPlayerManager _playerManager;
        private readonly IGameConfiguration _configuration;

        public Chat_GlobalChat_Handler(IPlayerManager playerManager, IGameConfiguration configuration, ILogger log) : base(log)
        {
            _playerManager = playerManager;
            _configuration = configuration;
        }

        protected override async Task Handle(GlobalChatEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            if (e.HasItemLinked)
            {
                await _playerManager.BroadcastAsync(player.GenerateSayItemPacket(_configuration.GlobalChatPrefix, e.Message, e.LinkedItem));
                return;
            }

            await _playerManager.BroadcastAsync(player.GenerateGlobalSayPacket(_configuration.GlobalChatPrefix, e.Message));
        }
    }
}