using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.NpcDialog.Events;
using ChickenAPI.Game.Features.NpcDialog.Handlers;

namespace ChickenAPI.Game.Features.NpcDialog
{
    public class NpcDialogEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<NpcDialogEventArgs>();

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case NpcDialogEventArgs dialogevent:
                    HandleNrun(entity as IPlayerEntity, dialogevent);
                    break;
            }
        }

        private static void HandleNrun(IPlayerEntity player, NpcDialogEventArgs args)
        {
            switch (args.DialogId)
            {
                // TODO

                // Basical 
                case 302: // Graham " Dialoguer " 
                    Log.Info($"Je discute avec Graham.");
                    break;

                case 301: // Graham " Pierre sacrée → vers l'archip  " 
                case 16: // Téléporteur ( Zap ) qui Tp Mt Krem + Port Alveus
                    TeleportTo(player, args);
                    break;
                    
                default:
                    Log.Info($" No Handler for {args.DialogId} {args.Type} {args.Value} {args.NpcId} : Mashalux Beaugosse va .");
                    break;
            }
        }

        private static void TeleportTo(IPlayerEntity player, NpcDialogEventArgs args)
        {
            TeleporterHandler.OnNpcDialogTeleport(player, args);
        }

    }
}