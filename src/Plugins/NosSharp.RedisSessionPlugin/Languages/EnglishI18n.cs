using System;
using System.Collections.Generic;
using ChickenAPI.Core.i18n;

namespace SaltyEmu.RedisWrappers.Languages
{
    public class EnglishI18n
    {
        public static Dictionary<PlayerMessages, string> Languages = new Dictionary<PlayerMessages, string>
        {
            { PlayerMessages.CHARACTER_NAME_ALREADY_TAKEN, "Character name is already taken" }
        };
    }
}