using ChickenAPI.Core.i18n;

namespace ChickenAPI.Game._i18n
{
    public interface IGameLanguageService
    {
        /// <summary>
        ///     Will return the string by its Key & Region
        ///     Used for plugins mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        string GetLanguage(string key, LanguageKey language);

        /// <summary>
        ///     Will return the string by its key & region
        ///     Used for ChickenAPI mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetLanguage(PlayerMessages key, LanguageKey type);

        /// <summary>
        ///     Will register the key and value by its region type
        ///     Used for plugins mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        void SetLanguage(string key, string value, LanguageKey type);

        /// <summary>
        ///     Will register the key and value by its region type
        ///     Used for ChickenAPI mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        void SetLanguage(PlayerMessages key, string value, LanguageKey type);
    }
}