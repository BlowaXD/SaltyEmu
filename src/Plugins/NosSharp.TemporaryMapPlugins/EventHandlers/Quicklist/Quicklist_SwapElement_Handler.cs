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
    public class Quicklist_SwapElement_Handler : GenericEventPostProcessorBase<QuicklistSwapElementEvent>
    {
        public Quicklist_SwapElement_Handler(ILogger log) : base(log)
        {
        }

        protected override async Task Handle(QuicklistSwapElementEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            // check sp
            CharacterQuicklistDto qlFrom = player.Quicklist.Quicklist.FirstOrDefault(n => n.Q1 == e.Data1 && n.Q2 == e.Data2);

            if (qlFrom == null)
            {
                // modified packet
                return;
            }

            // check sp
            CharacterQuicklistDto qlTo = player.Quicklist.Quicklist.FirstOrDefault(s => s.Q1 == e.Q1 && s.Q2 == e.Q2);

            if (qlTo == null)
            {
                await player.SendPacketAsync(player.Quicklist.GenerateRemoveQSetPacket(qlFrom.Q1, qlFrom.Q2));
            }
            else
            {
                qlTo.Q1 = qlFrom.Q1;
                qlTo.Q1 = qlFrom.Q2;
            }


            qlFrom.Q1 = e.Data1;
            qlFrom.Q2 = e.Data2;


            await player.SendPacketAsync(player.Quicklist.GenerateQSetPacket(qlFrom));
        }
    }
}