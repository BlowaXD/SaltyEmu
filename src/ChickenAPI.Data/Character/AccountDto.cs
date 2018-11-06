using ChickenAPI.Enums;
using ChickenAPI.Game.Permissions;

namespace ChickenAPI.Data.Character
{
    public class AccountDto : IMappedDto
    {
        /// <summary>
        ///     Account Id
        /// </summary>
        public long Id { get; set; }

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
        ///     Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Email used at registration
        /// </summary>
        public string RegistrationEmail { get; set; }

        /// <summary>
        ///     Ip used at registration
        /// </summary>
        public string RegistrationIp { get; set; }

        /// <summary>
        ///     Used for validation at registration
        /// </summary>
        public string RegistrationToken { get; set; }
    }
}