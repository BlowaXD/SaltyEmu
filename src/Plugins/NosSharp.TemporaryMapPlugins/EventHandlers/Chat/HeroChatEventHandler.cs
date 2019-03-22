using ChickenAPI.Core.Events;
using ChickenAPI.Game.Chat.Events;
using ChickenAPI.Game.Entities.Player;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Game.Configuration;
using ChickenAPI.Game.Extensions.PacketGeneration;

namespace SaltyEmu.BasicPlugin.EventHandlers.Chat
{
    public class HeroChatEventHandler : GenericEventPostProcessorBase<ChatHeroEvent>
    {
        private readonly IGameConfiguration _configuration;

        public HeroChatEventHandler(IGameConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override async Task Handle(ChatHeroEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }
            // todo only top 3 of hero reputation
            
            await player.BroadcastAsync(player.GenerateHeroPacket(e.Message));
        }
    }
}