﻿using ChickenAPI.Enums.Game.Effects;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Effects;
using ChickenAPI.Packets.Game.Client.Player;

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