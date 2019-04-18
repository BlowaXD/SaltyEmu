using System.Collections.Generic;
using System.Linq;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Entities.Mates;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class PlayerMatesExtensions
    {
        public static IEnumerable<IMateEntity> GetMateByPage(this IPlayerEntity player, MateType type, byte page) => player.Mates.Where(s => s.Mate.MateType == type).Skip(page * 10).Take(10);

        public static IEnumerable<IMateEntity> GetMateByMateType(this IPlayerEntity player, MateType type) => player.Mates.Where(s => s.Mate.MateType == type);
    }
}