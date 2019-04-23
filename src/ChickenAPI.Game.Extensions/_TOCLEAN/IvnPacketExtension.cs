using System.Collections.Generic;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums.Game.Items;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Inventory;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class IvnPacketExtension
    {
        public static IvnPacket GenerateEmptyIvnPacket(this IPlayerEntity player, PocketType type, short slot) =>
            new IvnPacket
            {
                Type = type,
                IvnSubPackets = new List<IvnSubPacket>
                {
                    new IvnSubPacket()
                    {
                        VNum = -1,
                        Slot = slot,
                        UpgradeDesign = 0,
                        RareAmount = 0,
                        SecondUpgrade = 0,
                    }
                }
            };

        public static IvnPacket GenerateIvnPacket(this ItemInstanceDto itemInstance)
        {
            switch (itemInstance.Type)
            {
                case PocketType.Specialist:
                    return new IvnPacket
                    {
                        Type = itemInstance.Type,
                        IvnSubPackets = new List<IvnSubPacket>
                        {
                            new IvnSubPacket
                            {
                                VNum = (short)itemInstance.ItemId,
                                UpgradeDesign = itemInstance.Upgrade,
                                Slot = itemInstance.Slot,
                                RareAmount = itemInstance.Rarity,
                                SecondUpgrade = itemInstance.SpecialistUpgrade,
                            }
                        }
                    };

                case PocketType.Equipment:
                    return new IvnPacket
                    {
                        Type = itemInstance.Type,
                        IvnSubPackets = new List<IvnSubPacket>
                        {
                            new IvnSubPacket
                            {
                                VNum = (short)itemInstance.ItemId,
                                UpgradeDesign = itemInstance.Upgrade,
                                Slot = itemInstance.Slot,
                                RareAmount = itemInstance.Rarity,
                            }
                        }
                    };
                case PocketType.Main:
                case PocketType.Etc:
                    return itemInstance.GenerateMainIvnPacket();

                default:
                    //Log.Info($"{itemInstance.Type} not implemented");
                    return null;
            }
        }

        private static IvnPacket GenerateMainIvnPacket(this ItemInstanceDto itemInstance)
        {
            return new IvnPacket
            {
                Type = itemInstance.Type,
                IvnSubPackets = new List<IvnSubPacket>
                {
                    new IvnSubPacket
                    {
                        VNum = (short)itemInstance.ItemId,
                        UpgradeDesign = 0,
                        Slot = itemInstance.Slot,
                        RareAmount = itemInstance.Amount,
                    }
                }
            };
        }
    }
}