using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Game.NpcDialog.Handling;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.NpcDialogHandlers
{
    public class DialogHandler
    {
        // private static readonly Logger Log = Logger.GetLogger<DialogHandler>();

        [NpcDialogHandler(1)]
        public static void ChangeJobMiMi(IPlayerEntity player, NpcDialogEvent args)
        {
            if (player.Character.Class != CharacterClassType.Adventurer)
            {
                //await session.SendPacketAsync(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ADVENTURER"), 0));
                return;
            }

            if (player.Character.Level < 15 || player.Character.JobLevel < 20)
            {
                //await session.SendPacketAsync(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("LOW_LVL"), 0));
                return;
            }

            if (args.Type == (byte)player.Character.Class)
            {
                return;
            }

            if (args.Type > 4)
            {
                return;
            }

            player.ChangeClass((CharacterClassType)args.Type).ConfigureAwait(false).GetAwaiter().GetResult();
            // TODO : LATER
            /* if (Session.Character.Inventory.All(i => i.Type == PocketType.Wear))
             {
                 await session.SendPacketAsync(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("EQ_NOT_EMPTY"), 0));
                 return;
             }

                 Session.Character.Inventory.AddNewToInventory((short)(4 + (packet.Type * 14)), type: PocketType.Wear);
                 Session.Character.Inventory.AddNewToInventory((short)(81 + (packet.Type * 13)), type: PocketType.Wear);
                 switch (packet.Type)
                 {
                     case 1:
                         Session.Character.Inventory.AddNewToInventory(68, type: PocketType.Wear);
                         Session.Character.Inventory.AddNewToInventory(2082, 10);
                         break;

                     case 2:
                         Session.Character.Inventory.AddNewToInventory(78, type: PocketType.Wear);
                         Session.Character.Inventory.AddNewToInventory(2083, 10);
                         break;

                     case 3:
                         Session.Character.Inventory.AddNewToInventory(86, type: PocketType.Wear);
                         break;

                     default:
                         return;
                 }
                 Session.CurrentMapInstance?.Broadcast(Session.Character.GenerateEq());
                 await session.SendPacketAsync(Session.Character.GenerateEquipment());*/
        }
    }
}