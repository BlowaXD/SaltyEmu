using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Game.NpcDialog.Handling;

namespace SaltyEmu.BasicPlugin.NpcDialogHandlers
{
    public class FamilyHandler
    {
        private static readonly Logger Log = Logger.GetLogger<FamilyHandler>();

        [NpcDialogHandler(23)]
        public static void CreateFamily(IPlayerEntity player, NpcDialogEvent args)
        {
        }

        [NpcDialogHandler(1600)]
        public static void OnpenFamilyWarehouse(IPlayerEntity player, NpcDialogEvent args)
        {
            // await session.SendPacketAsync(Session.Character.OpenFamilyWarehouse());
        }

        [NpcDialogHandler(1601)]
        public static void OpenFamilyWarehouseHist(IPlayerEntity player, NpcDialogEvent args)
        {
            //Session.SendPackets(Session.Character.OpenFamilyWarehouseHist());
        }

        [NpcDialogHandler(1602)]
        public static void UpgradeFamilyWarehouse(IPlayerEntity player, NpcDialogEvent args)
        {
            // = 21
        }

        [NpcDialogHandler(1603)]
        public static void UpgradeFamilyWarehouse2(IPlayerEntity player, NpcDialogEvent args)
        {
            // = 21 -> 49
        }

        [NpcDialogHandler(1604)]
        public static void UpgradeFamilySize(IPlayerEntity player, NpcDialogEvent args)
        {
        }

        [NpcDialogHandler(1605)]
        public static void UpgradeFamilySize2(IPlayerEntity player, NpcDialogEvent args)
        {
            if (player.Family.MaxSize > 100 && player.Family.FamilyLevel <= 9)
            {
                return;
            }
            if (player.FamilyCharacter.Authority != FamilyAuthority.Head)
            {
                /* await session.SendPacketAsync(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("ONLY_HEAD_CAN_BUY"), 10));
                    await session.SendPacketAsync(UserInterfaceHelper.GenerateModal(Language.Instance.GetMessageFromKey("ONLY_HEAD_CAN_BUY"), 1));
                    */
                return;
            }

            if (5000000 >= player.Character.Gold)
            {
                //await session.SendPacketAsync(Session.Character.GenerateSay(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 10));
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