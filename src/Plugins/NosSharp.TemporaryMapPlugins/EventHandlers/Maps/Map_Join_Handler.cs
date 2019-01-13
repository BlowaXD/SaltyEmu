using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Maps.Events;
using ChickenAPI.Game.Maps.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.PacketHandling.Extensions;
using ChickenAPI.Game.Player.Extension;

namespace SaltyEmu.BasicPlugin.EventHandlers.Maps
{
    public class Map_Join_Handler : GenericEventPostProcessorBase<MapJoinEvent>
    {
        protected override async Task Handle(MapJoinEvent e, CancellationToken cancellation)
        {

            if (!(e.Sender is IPlayerEntity player))
            {
                await BroadcastInPacket(e);
                return;
            }

            await SendCharacterInformationsPackets(player);
            await SendRightPackets(player);
            await SendGroupPackets(player);
            await SendMapInfosPackets(player, e);
        }
        private static async Task SendMapInfosPackets(IPlayerEntity player, MapJoinEvent join)
        {
            // map design objects (mltobj)
            await player.SendPacketsAsync(join.Map.GetPortalsPackets());
            // GenerateWp()
            await player.SendPacketsAsync(join.Map.GetEntitiesPackets());
            await player.SendPacketsAsync(join.Map.GetPairyPackets(player));
            await player.SendPacketsAsync(join.Map.GetShopsPackets());

            // user shop
            // usershop (pflag) minimap
        }

        private static async Task SendCharacterInformationsPackets(IPlayerEntity player)
        {
            await player.SendPacketAsync(player.GenerateCInfoPacket());
            await player.SendPacketAsync(player.GenerateCModePacket());
            await player.SendPacketAsync(player.GenerateEqPacket());
            await player.SendPacketAsync(player.GenerateEquipmentPacket());

            await player.ActualizeUiExpBar();
            await player.SendPacketAsync(player.GenerateStatPacket());
        }

        private static async Task SendRightPackets(IPlayerEntity player)
        {
            await player.SendPacketAsync(player.GenerateAtPacket());
            await player.ActualizePlayerCondition();
            await player.SendPacketAsync(player.GenerateCMapPacket());
            await player.SendPacketAsync(player.GenerateStatCharPacket());
            await player.SendPacketAsync(player.GeneratePairyPacket());
        }

        private static async Task SendGroupPackets(IPlayerEntity player)
        {
            //player.SendPackets(player.GeneratePstPackets());
            // Act6() : Act()
            await player.ActualizeUiGroupIcons();
            await player.ActualizeMateListInPlayerPanel();
            // ScpStcPacket
            // FcPacket
            // Act4Raid ? DgPacket() : RaidMbf
            // MapDesignObjects()
            // MapDesignObjectsEffects
            // MapItems()
            // Gp()
            //SendPacket(new RsfpPacket()); // Minimap Position
            await player.SendPacketAsync(player.GenerateCondPacket());
            await player.BroadcastAsync(player.GenerateInPacket());
            await player.SendPacketAsync(player.GenerateStatPacket());
        }

        private Task BroadcastInPacket(MapJoinEvent e) => e.Map.BroadcastAsync(e.Sender.GenerateInPacket());
    }
}