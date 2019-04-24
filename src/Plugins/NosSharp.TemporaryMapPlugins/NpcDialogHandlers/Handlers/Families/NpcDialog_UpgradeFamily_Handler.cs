using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.NpcDialog;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Game._i18n;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.NpcDialogHandlers.Handlers.Families
{
    public class NpcDialog_UpgradeFamily_Handler : INpcDialogAsyncHandler
    {
        public long HandledId => 1604;

        public async Task Execute(IPlayerEntity player, NpcDialogEvent e)
        {
            if (player.Family.MaxSize > 70 && player.Family.FamilyLevel <= 5)
            {
                return;
            }

            if (player.FamilyCharacter.Authority != ChickenAPI.Enums.Game.Families.FamilyAuthority.Head)
            {
                await player.SendChatMessageAsync(PlayerMessages.FAMILY_YOU_NEED_TO_BE_LEADER, SayColorType.Yellow);
                await player.SendModalAsync(PlayerMessages.FAMILY_YOU_NEED_TO_BE_LEADER, ModalPacketType.Default);
                return;
            }

            if (5000000 >= player.Character.Gold)
            {
                await player.SendChatMessageAsync(PlayerMessages.YOU_DONT_HAVE_ENOUGH_GOLD, SayColorType.Yellow);
                return;
            }

            await player.GoldLess(5000000);
            player.Family.MaxSize = 70;
            /*FamilyDTO fam = Session.Character.Family;
            DAOFactory.FamilyDAO.InsertOrUpdate(ref fam);
            ServerManager.Instance.FamilyRefresh(Session.Character.Family.FamilyId);*/
        }
    }
}