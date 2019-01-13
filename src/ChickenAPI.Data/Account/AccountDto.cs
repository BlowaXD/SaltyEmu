using ChickenAPI.Core.i18n;
using ChickenAPI.Enums;

namespace ChickenAPI.Data.Account
{
    public class AccountDto : IMappedDto
    {
        public AuthorityType Authority { get; set; }

        /// <summary>
        ///     Account Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Hashed to Sha512
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// </summary>
        public LanguageKey Language { get; set; }

        /// <summary>
        ///     Account Id
        /// </summary>
        public long Id { get; set; }
    }
}