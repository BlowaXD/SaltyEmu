using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.GuriHandling.Args;
using ChickenAPI.Game.Features.GuriHandling.Handling;
using ChickenAPI.Game.Features.NpcDialog.Handlers;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Game.Features.GuriHandling.Handlers
{
    public class EmoticonGuriHandler
    {
        private static readonly Logger Log = Logger.GetLogger<TeleporterHandler>();

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        [PermissionsRequirements(PermissionType.GURI_EMOTICON)]
        [GuriEffect(10)]
        public static void OnEmoticonRequest(IPlayerEntity player, GuriEventArgs e)
        {
            if (!(e.Data >= 973 && e.Data <= 999 && !player.Character.EmoticonsBlocked))
            {
                return;
            }


            // todo receiver type
            player.Broadcast(player.EmojiToEffectPacket((EmojiType)e.Data));

            Log.Info($"[GURI][EMOTICON] {player.Character.Name} used emoji : ");
        }
    }
}