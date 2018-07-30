using System.Text;
using ChickenAPI.Data.Language;
using ChickenAPI.Enums;

namespace ChickenAPI.Data.AccessLayer.Server
{
    public interface ILanguageService
    {
        /// <summary>
        ///     Will return the string by its Key & Region
        /// Used for plugins mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetLanguage(string key, RegionType type);

        /// <summary>
        /// Will return the string by its key & region
        /// Used for ChickenAPI mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetLanguage(LanguageKeys key, RegionType type);

        /// <summary>
        /// Will register the key and value by its region type
        /// Used for plugins mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="encodingInfo"></param>
        void SetLanguage(string key, string value, RegionType type);

        /// <summary>
        /// Will register the key and value by its region type
        /// Used for ChickenAPI mainly
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="encodingInfo"></param>
        void SetLanguage(LanguageKeys key, string value, RegionType type);
    }
}