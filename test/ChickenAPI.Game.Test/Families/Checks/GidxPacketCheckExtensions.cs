using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Client.Families;

namespace ChickenAPI.Game.Test.Families.Checks
{
    public static class GidxPacketCheckExtensions
    {
        /// <summary>
        /// Checks if the packet is a "default packet"
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsEmptyFamilyPacket(this GidxPacket packet, IPlayerEntity player)
        {
            return packet.VisualType == player.Type &&
                packet.VisualId == player.Id &&
                packet.FamilyId == -1 &&
                packet.FamilyName == "-" &&
                packet.FamilyLevel == 0;
        }

        public static bool IsPlayerFamilyPacket(this GidxPacket packet, IPlayerEntity player)
        {
            return packet.VisualType == player.Type &&
                packet.VisualId == player.Id &&
                packet.FamilyId == player.Family.Id &&
                packet.FamilyName == player.Family.Name;
        }
    }
}