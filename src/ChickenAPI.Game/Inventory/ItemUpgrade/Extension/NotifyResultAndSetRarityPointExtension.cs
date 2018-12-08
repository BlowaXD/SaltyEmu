using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Inventory.ItemUpgrade.Events;
using ChickenAPI.Game.Player.Extension;

namespace ChickenAPI.Game.Inventory.ItemUpgrade.Extension
{
    public static class NotifyResultAndSetRarityPointExtension
    {
        public static void Rarify(this RarifyEventArgs e, IPlayerEntity player, sbyte rarity)
        {
            if (e.Mode != RarifyMode.Drop)
            {
                player.NotifyRarifyResult(rarity);
            }
            e.Item.Rarity = rarity;
            /* GenerateHeroicShell(protection);*/
            e.Item.SetRarityPoint();
        }

        public static void Fails(this RarifyEventArgs e, IPlayerEntity player)
        {
            if (e.Mode != RarifyMode.Drop)
            {
                switch (e.Protection)
                {
                    case RarifyProtection.BlueAmulet:
                    case RarifyProtection.RedAmulet:
                    case RarifyProtection.HeroicAmulet:
                    case RarifyProtection.RandomHeroicAmulet:
                        var amulets = player.Inventory.GetItemFromSlotAndType((short)EquipmentType.Amulet, InventoryType.Wear);
                        if (amulets == null)
                        {
                            return;
                        }

                        /* amulet.DurabilityPoint -= 1;
                         if (amulet.DurabilityPoint <= 0)
                         {
                             session.Character.DeleteItemByItemInstanceId(amulet.Id);
                             session.SendPacket($"info {Language.Instance.GetMessageFromKey("AMULET_DESTROYED")}");
                             session.SendPacket(session.Character.GenerateEquipment());
                         }*/
                        player.SendTopscreenMessage("AMULET_FAIL_SAVED", MsgPacketType.Whisper);
                        player.SendChatMessage("AMULET_FAIL_SAVED", SayColorType.Purple);
                        return;

                    case RarifyProtection.None:
                        /* session.Character.DeleteItemByItemInstanceId(Id);*/
                        //player.EmitEvent(new InventoryDestroyItemEvent { ItemInstance = e.Item });
                        player.SendTopscreenMessage("RARIFY_FAILED", MsgPacketType.Whisper);
                        player.SendChatMessage("RARIFY_FAILED", SayColorType.Purple);

                        return;
                }

                player.SendTopscreenMessage("RARIFY_FAILED_ITEM_SAVED", MsgPacketType.Whisper);
                player.SendChatMessage("RARIFY_FAILED_ITEM_SAVED", SayColorType.Purple);
                player.Broadcast(player.GenerateEffectPacket(3004));
                return;
            }
        }
    }
}