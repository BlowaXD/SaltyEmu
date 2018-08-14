using System.Text;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Features.Skills.Extensions
{
    public static class SkiPacketExtensions
    {
        public static SkiPacket GenerateSkiPacket(this IPlayerEntity player)
        {
            var tmp = new StringBuilder();

            // check sp


            return new SkiPacket
            {
                SkiPacketContent = tmp.ToString()
            };
        }
    }
}