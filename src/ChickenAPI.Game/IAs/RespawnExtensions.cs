using System.Threading.Tasks;
using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Inventory.Extensions;

namespace ChickenAPI.Game.IAs
{
    public static class RespawnExtensions
    {
        public static Task Respawn(this IBattleEntity entity)
        {
            entity.Hp = entity.HpMax;
            entity.Mp = entity.MpMax;
            return entity.CurrentMap.BroadcastAsync(entity.GenerateInPacket());
        }
    }
}