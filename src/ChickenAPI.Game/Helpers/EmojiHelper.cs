using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Enums.Game.Effects;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Packets.Game.Client._NotYetSorted;

namespace ChickenAPI.Game.Helpers
{
    public static class EmojiHelper
    {
        private const int EMOJI_EFFECT_OFFSET = 4099;

        public static EffectPacket EmojiToEffectPacket(this IEntity entity, EmojiType type)
        {
            return entity.GenerateEffectPacket((int)type + EMOJI_EFFECT_OFFSET);
        }
    }
}