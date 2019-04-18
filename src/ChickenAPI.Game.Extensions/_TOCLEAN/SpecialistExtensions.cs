using ChickenAPI.Game._ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets.Game.Server.Specialist;
using System.Threading.Tasks;

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

        public static Task SendSdAsync(this IPlayerEntity player, int cooldown)
        {
            return player.SendPacketAsync(player.GenerateSdPacket(cooldown));
        }

        public static SdPacket GenerateSdPacket(this IPlayerEntity player, int cooldown)
        {
            return new SdPacket
            {
                CoolDown = cooldown
            };
        }
    }
}