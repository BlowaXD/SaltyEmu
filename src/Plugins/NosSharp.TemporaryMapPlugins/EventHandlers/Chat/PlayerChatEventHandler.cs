using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Chat.Events;
using ChickenAPI.Game.Entities.Player;

namespace SaltyEmu.BasicPlugin.EventHandlers.Chat
{

    public class PlayerChatEventHandler : GenericEventPostProcessorBase<ChatGeneralEvent>
    {
        protected override async Task Handle(ChatGeneralEvent e, CancellationToken cancellation)
        {
            var sayPacket = new SayPacket
            {
                Type = SayColorType.White,
                Message = e.Message,
                VisualType = e.Sender.Type,
                VisualId = e.Sender.Id
            };
            await ((IPlayerEntity)e.Sender).BroadcastExceptSenderAsync(sayPacket);
        }
    }
}