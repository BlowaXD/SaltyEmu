using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.NpcDialog.Events;
using ChickenAPI.Game.NpcDialog.Handling;
using ChickenAPI.Packets.ClientPackets.Npcs;
using ChickenAPI.Packets.Enumerations;
using ChickenAPI.Packets.ServerPackets.Player;
using ChickenAPI.Packets.ServerPackets.Shop;
using ChickenAPI.Packets.ServerPackets.UI;

namespace SaltyEmu.BasicPlugin.NpcDialogHandlers
{
    public class WopenHandler
    {
        [NpcDialogHandler(2)]
        public static void UpgradeFromNpc(IPlayerEntity player, NpcDialogEvent args)
        {
            player.SendPacketAsync(new WopenPacket
            {
                Type = WindowType.UpgradeItem,
                Unknown = 0,
            });
        }

        [NpcDialogHandler(10)]
        public static void Cellon(IPlayerEntity player, NpcDialogEvent args)
        {
            player.SendPacketAsync(new WopenPacket
            {
                Type = WindowType.CellonItem,
                Unknown = 0
            });
        }

        [NpcDialogHandler(12)]
        public static void Idk(IPlayerEntity player, NpcDialogEvent args)
        {
            player.SendPacketAsync(new WopenPacket
            {
                Type = (WindowType)args.Type,
                Unknown = 0
            });
        }

        [NpcDialogHandler(14)]
        public static void Recipe(IPlayerEntity player, NpcDialogEvent args)
        {
            player.SendPacketAsync(new WopenPacket
            {
                Type = WindowType.Production,
                Unknown = 0
            });
            // Recipe
            /*string recipelist = "m_list 2";
                    if (npc != null)
                    {
                        List<Recipe> tps = npc.Recipes;
                        recipelist = tps.Where(s => s.Amount > 0).Aggregate(recipelist, (current, s) => current + $" {s.ItemVNum}");
                        recipelist += " -100";
                        await session.SendPacketAsync(recipelist);
                    }
                    */
        }

        /// <summary>
        /// This method show the Menu ( TimeCircle ) for enter in x instance { arena / arena familly / raimbow battle }
        /// </summary>
        /// <param name="player"></param>
        /// <param name="args"></param>
        [NpcDialogHandler(18)]
        public static void TimeCircle(IPlayerEntity player, NpcDialogEvent args)
        {
            player.SendPacketAsync(new RequestNpcPacket
            {
                Type = player.Type,
                TargetId = player.Id,
                Data = 17
            });
        }

        [NpcDialogHandler(60)]
        public static void OpenBazzar(IPlayerEntity player, NpcDialogEvent args)
        {
            /*
             * StaticBonusDTO medal = Session.Character.StaticBonusList.Find(s => s.StaticBonusType == StaticBonusType.BazaarMedalGold || s.StaticBonusType == StaticBonusType.BazaarMedalSilver);
                    byte Medal = 0;
                    int Time = 0;
                    if (medal != null)
                    {
                        Medal = medal.StaticBonusType == StaticBonusType.BazaarMedalGold ? (byte)MedalType.Gold : (byte)MedalType.Silver;
                        Time = (int)(medal.DateEnd - DateTime.Now).TotalHours;
                    }
                    await session.SendPacketAsync($"wopen 32 {Medal} {Time}");
                    */
            player.SendPacketAsync(new WopenPacket
            {
                Type = WindowType.NosBazaar,
                Unknown = 1
            });
        }
    }
}