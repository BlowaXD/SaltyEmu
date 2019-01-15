using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Core.i18n;

namespace ChickenAPI.Game._i18n
{
    public interface IStaticDataLanguageService
    {
        /// <summary>
        ///     Will return the string by its Key & Region
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        Task<string> GetLanguage(string key, LanguageKey language);

        /// <summary>
        ///     Will register the key and value by its region type
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetLanguage(string key, LanguageKey language, string value);

        /// <summary>
        /// Searches for all the static data names that are matching with the given search name
        /// </summary>
        /// <param name="search"></param>
        /// <param name="language"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<IEnumerable<string>> Search(string search, LanguageKey language, StaticDataNameSearchType type);
    }
}