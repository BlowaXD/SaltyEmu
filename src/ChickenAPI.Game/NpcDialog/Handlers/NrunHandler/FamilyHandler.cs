using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Game.Player.Extension;

namespace ChickenAPI.Game.NpcDialog.Handlers
{
    public class FamilyHandler
    {
        private static readonly Logger Log = Logger.GetLogger<FamilyHandler>();

        [NpcDialogHandler(23)]
        public static void CreateFamily(IPlayerEntity player, NpcDialogEventArgs args)
        {
        }

        [NpcDialogHandler(1600)]
        public static void OnpenFamilyWarehouse(IPlayerEntity player, NpcDialogEventArgs args)
        {
            // Session.SendPacket(Session.Character.OpenFamilyWarehouse());
        }

        [NpcDialogHandler(1601)]
        public static void OpenFamilyWarehouseHist(IPlayerEntity player, NpcDialogEventArgs args)
        {
            //Session.SendPackets(Session.Character.OpenFamilyWarehouseHist());
        }

        [NpcDialogHandler(1602)]
        public static void UpgradeFamilyWarehouse(IPlayerEntity player, NpcDialogEventArgs args)
        {
            // = 21
        }

        [NpcDialogHandler(1603)]
        public static void UpgradeFamilyWarehouse2(IPlayerEntity player, NpcDialogEventArgs args)
        {
            // = 21 -> 49
        }

        [NpcDialogHandler(1604)]
        public static void UpgradeFamilySize(IPlayerEntity player, NpcDialogEventArgs args)
        {
            if (player.Family.MaxSize > 70 && player.Family.FamilyLevel <= 5)
            {
                return;
            }
            if (player.FamilyCharacter.Authority != Enums.Game.Families.FamilyAuthority.Head)
            {
                /* Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("ONLY_HEAD_CAN_BUY"), 10));
                    Session.SendPacket(UserInterfaceHelper.GenerateModal(Language.Instance.GetMessageFromKey("ONLY_HEAD_CAN_BUY"), 1));
                    */
                return;
            }

            if (5000000 >= player.Character.Gold)
            {
                //Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 10));
                return;
            }
            player.GoldLess(5000000);
            player.Family.MaxSize = 70;
            /*FamilyDTO fam = Session.Character.Family;
            DAOFactory.FamilyDAO.InsertOrUpdate(ref fam);
            ServerManager.Instance.FamilyRefresh(Session.Character.Family.FamilyId);*/
        }

        [NpcDialogHandler(1605)]
        public static void UpgradeFamilySize2(IPlayerEntity player, NpcDialogEventArgs args)
        {
            if (player.Family.MaxSize > 100 && player.Family.FamilyLevel <= 9)
            {
                return;
            }
            if (player.FamilyCharacter.Authority != Enums.Game.Families.FamilyAuthority.Head)
            {
                /* Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("ONLY_HEAD_CAN_BUY"), 10));
                    Session.SendPacket(UserInterfaceHelper.GenerateModal(Language.Instance.GetMessageFromKey("ONLY_HEAD_CAN_BUY"), 1));
                    */
                return;
            }

            if (5000000 >= player.Character.Gold)
            {
                //Session.SendPacket(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 10));
                return;
            }
            player.GoldLess(10000000);
            player.Family.MaxSize = 100;
            /*FamilyDTO fam = Session.Character.Family;
            DAOFactory.FamilyDAO.InsertOrUpdate(ref fam);
            ServerManager.Instance.FamilyRefresh(Session.Character.Family.FamilyId);*/
        }
    }
}