using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.GuriHandling.Events;
using ChickenAPI.Game.GuriHandling.Handling;
using ChickenAPI.Game.Helpers;

namespace SaltyEmu.BasicPlugin.GuriHandlers
{
    public class EmoticonGuriHandler
    {
        // private static readonly Logger Log = Logger.GetLogger<EmoticonGuriHandler>();

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="e"></param>
        [GuriEffect(10)]
        public static void OnEmoticonRequest(IPlayerEntity player, GuriEvent e)
        {
            if (!(e.Data >= 973 && e.Data <= 999 && !player.Character.EmoticonsBlocked))
            {
                return;
            }

            // todo receiver type
            player.BroadcastAsync(player.EmojiToEffectPacket((EmojiType)e.Data)).ConfigureAwait(false).GetAwaiter().GetResult();

            // Log.Info($"[GURI][EMOTICON] {player.Character.Name} used emoji : ");
        }
    }
}