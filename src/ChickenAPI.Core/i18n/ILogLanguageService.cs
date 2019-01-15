namespace ChickenAPI.Core.i18n
{
    public interface ILogLanguageService
    {
        /// <summary>
        ///     Will return the string by its key & region
        ///     Used for ChickenAPI mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetLanguage(LogI18NKeys key, LanguageKey type);

        /// <summary>
        ///     Will register the key and value by its region type
        ///     Used for ChickenAPI mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        void SetLanguage(LogI18NKeys key, string value, LanguageKey type);
    }
}