using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.PacketHandling.Extensions;

namespace ChickenAPI.Game.IAs
{
    public static class RespawnExtensions
    {
        public static void Respawn(this IBattleEntity entity)
        {
            entity.Hp = entity.HpMax;
            entity.Mp = entity.MpMax;
            entity.CurrentMap.Broadcast(entity.GenerateInPacket());
        }
    }
}