using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog;
using ChickenAPI.Game.NpcDialog.Events;

namespace SaltyEmu.BasicPlugin.EventHandlers.NpcDialog
{
    public class NpcDialogEventHandler : GenericEventPostProcessorBase<NpcDialogEvent>
    {
        private readonly INpcDialogHandler _npcDialogHandler;

        public NpcDialogEventHandler(INpcDialogHandler npcDialogHandler)
        {
            _npcDialogHandler = npcDialogHandler;
        }

        protected override Task Handle(NpcDialogEvent e, CancellationToken cancellation) => Task.Run(() =>_npcDialogHandler.Execute(e.Sender as IPlayerEntity, e));
    }
}