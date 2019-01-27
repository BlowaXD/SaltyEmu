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
        private readonly INpcDialogHandlerContainer _npcDialogHandler;

        public NpcDialogEventHandler(INpcDialogHandlerContainer npcDialogHandler)
        {
            _npcDialogHandler = npcDialogHandler;
        }

        protected override Task Handle(NpcDialogEvent e, CancellationToken cancellation) => _npcDialogHandler.Execute(e.Sender as IPlayerEntity, e);
    }
}