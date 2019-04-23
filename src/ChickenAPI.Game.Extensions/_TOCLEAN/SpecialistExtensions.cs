using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using System.Threading.Tasks;
using ChickenAPI.Packets.ServerPackets.Specialists;

namespace ChickenAPI.Game.Specialists.Extensions
{
    public static class SpecialistExtensions
    {
        public static void WearSp(ISpecialistEntity entity)
        {
        }

        public static void SetMorph(this IMorphableEntity entity, short id)
        {
            entity.MorphId = id;
        }

        public static Task SendSdAsync(this IPlayerEntity player, short cooldown)
        {
            return player.SendPacketAsync(player.GenerateSdPacket(cooldown));
        }

        public static SdPacket GenerateSdPacket(this IPlayerEntity player, short cooldown)
        {
            return new SdPacket
            {
                Cooldown = cooldown
            };
        }
    }
}