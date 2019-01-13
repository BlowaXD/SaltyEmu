using System;

namespace NosWings.UserService.Entities
{
    public class User
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string RegistrationEmail { get; set; }

        public string ActualEmail { get; set; }

        public long Money { get; set; }

        public string DiscordUuid { get; set; }

        public string Password { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
