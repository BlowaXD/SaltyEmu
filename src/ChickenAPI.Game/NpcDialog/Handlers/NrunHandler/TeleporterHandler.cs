using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Game.Permissions;
using ChickenAPI.Game.Player.Extension;
using System;
using System.Diagnostics;

namespace ChickenAPI.Game.NpcDialog.Handlers
{
    public class TeleporterHandler
    {
        private static readonly Logger Log = Logger.GetLogger<TeleporterHandler>();

        /// <summary>
        /// This method will teleport the requester to Krem or Alveus, depending on which dialog type he choosed
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(16)]
        public static void OnNpcDialogTeleport(IPlayerEntity player, NpcDialogEventArgs args)
        {
            if (args.Type < 0)
            {
                return;
            }

            if ((player.Character.Gold <= 1000 * args.Type))
            {
                // No Money -> SendMsg(NOMONEY);
                Log.Info($"[TELEPORT][NO-MONEY] {player.Character.Name}");
                return;
            }

            player.GoldLess(1000 * args.Type);

            switch (args.Type)
            {
                case 1: // TeleportZapMtKrem
                    TeleportationHelper.TeleportTo(player, 20, 10, 91);
                    break;

                case 2: // TeleportZapPortsAlveus
                    TeleportationHelper.TeleportTo(player, 145, 8, 107);
                    break;
            }
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(17)]
        public static void EnterArenaInstance(IPlayerEntity player, NpcDialogEventArgs args)
        {
            double currentRunningSeconds = (DateTime.Now - Process.GetCurrentProcess().StartTime.AddSeconds(-50)).TotalSeconds;
            double timeSpanSinceLastPortal = currentRunningSeconds - player.LastPortal;
            /* if (!(timeSpanSinceLastPortal >= 4) || !Session.HasCurrentMapInstance || ServerManager.Instance.ChannelId == 51 ||
                 Session.CurrentMapInstance.MapInstanceId == ServerManager.Instance.ArenaInstance.MapInstanceId ||
                 Session.CurrentMapInstance.MapInstanceId == ServerManager.Instance.FamilyArenaInstance.MapInstanceId)
             {
                 Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("CANT_MOVE"), 10));
                 return;
             }*/

            if (args.Type < 0)
            {
                return;
            }

            if ((player.Character.Gold <= 500 * (1 * args.Type)))
            {
                // No Money -> SendMsg(NOMONEY);
                Log.Info($"[TELEPORT][NO-MONEY] {player.Character.Name}");
                return;
            }

            player.GoldLess(500 * (1 * args.Type));

            // MapCell pos = packet.Type == 0 ? ServerManager.Instance.ArenaInstance.Map.GetRandomPosition() : ServerManager.Instance.FamilyArenaInstance.Map.GetRandomPosition();
            // ServerManager.Instance.ChangeMapInstance(Session.Character.CharacterId, packet.Type == 0 ? ServerManager.Instance.ArenaInstance.MapInstanceId : ServerManager.Instance.FamilyArenaInstance.MapInstanceId, pos.X, pos.Y);

            TeleportationHelper.TeleportTo(player, (short)(args.Type == 0 ? 2006 : 2106), (short)(args.Type == 0 ? 38 : 2007), (short)(args.Type == 0 ? 38 : 2007));
            player.LastPortal = currentRunningSeconds;
        }

        /// <summary>
        /// This method will teleport the requester to Act 6
        /// It requires the player to be near Graham
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(301)]
        public static void OnGrahamDialogTeleport(IPlayerEntity player, NpcDialogEventArgs args)
        {
            Log.Info($"[TELEPORT][GRAHAM] {player.Character.Name}");
            // need to provide implementation
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(26)]
        public static void TeleportUnknow(IPlayerEntity player, NpcDialogEventArgs args)
        {
            // Idk Whats is Nrun 26 -> I thinks is For Ships ( Act4 or Act5 )
            if (args.Type < 0)
            {
                return;
            }

            if ((player.Character.Gold <= 5000 * args.Type))
            {
                // No Money -> SendMsg(NOMONEY);
                Log.Info($"[TELEPORT][NO-MONEY] {player.Character.Name}");
                return;
            }

            player.GoldLess(5000 * args.Type);

            //TeleportationHelper.TeleportTo(player, 20, 10, 91);
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(45)]
        public static void TeleportUnknow2(IPlayerEntity player, NpcDialogEventArgs args)
        {
            // Same Here
            if (args.Type < 0)
            {
                return;
            }

            if (player.Character.Gold <= 500)
            {
                // No Money -> SendMsg(NOMONEY);
                Log.Info($"[TELEPORT][NO-MONEY] {player.Character.Name}");
                return;
            }

            player.GoldLess(500);

            //TeleportationHelper.TeleportTo(player, 20, 10, 91);
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(132)]
        public static void TeleportUnknow3(IPlayerEntity player, NpcDialogEventArgs args)
        {
            // Same Here
            if (args.Type < 0)
            {
                return;
            }

            //TeleportationHelper.TeleportTo(player, 20, 10, 91);
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(301)]
        public static void TeleportUnknow4(IPlayerEntity player, NpcDialogEventArgs args)
        {
            // Same Here
            if (args.Type < 0)
            {
                return;
            }

            //TeleportationHelper.TeleportTo(player, 20, 10, 91);
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(150)]
        public static void EnterLod(IPlayerEntity player, NpcDialogEventArgs args)
        {
            if (player.Level <= 55)
            {
                //Session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("LOD_REQUIERE_LVL"), 0));
                return;
            }
            if (player?.Family == null)
            {
                /* Session.SendPacket(
                     UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NEED_FAMILY"),
                         0));*/
                return;
            }
            /* if (player.Family?.LandOfDeath == null)
             {
                 Session.Character.Family.LandOfDeath =
                     ServerManager.GenerateMapInstance(150, MapInstanceType.LodInstance,
                         new InstanceBag());
             }

             if (Session.Character?.Family?.LandOfDeath != null)
             {
                 ServerManager.Instance.ChangeMapInstance(Session.Character.CharacterId, Session.Character.Family.LandOfDeath.MapInstanceId, 153, 145);
             }*/

            TeleportationHelper.TeleportTo(player, 150, 153, 145);
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(5004)]
        public static void AlveusFromAct4(IPlayerEntity player, NpcDialogEventArgs args)
        {
            TeleportationHelper.TeleportTo(player, 145, 50, 41);
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(5011)]
        public static void GoToAct5(IPlayerEntity player, NpcDialogEventArgs args)
        {
            TeleportationHelper.TeleportTo(player, 170, 127, 46);
        }

        [PermissionsRequirements(PermissionType.NPC_DIALOG_TELEPORT)]
        [NpcDialogHandler(5012)]
        public static void TpUnknow(IPlayerEntity player, NpcDialogEventArgs args)
        {
            //TeleportationHelper.TeleportTo(player, 170, 127, 46);
        }
    }
}