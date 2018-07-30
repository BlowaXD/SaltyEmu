using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Enums;

namespace NosSharp.DatabasePlugin.Models.Character
{
    [Table("account")]
    public class AccountModel : IMappedDto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public AuthorityType Authority { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string RegistrationEmail { get; set; }


        [MaxLength(50)]
        public string RegistrationIp { get; set; }


        [MaxLength(32)]
        public string RegistrationToken { get; set; }

        public List<CharacterModel> Characters { get; set; }
    }
}