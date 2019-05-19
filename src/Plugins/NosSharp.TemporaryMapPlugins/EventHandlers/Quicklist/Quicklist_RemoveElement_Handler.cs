using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Quicklist.Events;
using ChickenAPI.Game.Quicklist.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Quicklist
{
    public class Quicklist_RemoveElement_Handler : GenericEventPostProcessorBase<QuicklistRemoveElementEvent>
    {
        public Quicklist_RemoveElement_Handler(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(QuicklistRemoveElementEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            CharacterQuicklistDto qlFrom = player.Quicklist.Quicklist.FirstOrDefault(n => n.Q1 == e.Data1 && n.Q2 == e.Data2);

            if (qlFrom == null)
            {
                // can't remove what does not exist
                return;
            }

            player.Quicklist.Quicklist.Remove(qlFrom);
            await player.SendPacketAsync(player.Quicklist.GenerateRemoveQSetPacket(qlFrom.Q1, qlFrom.Q2));
        }
    }
}