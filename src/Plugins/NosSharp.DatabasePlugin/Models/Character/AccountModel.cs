using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.i18n;
using ChickenAPI.Enums;
using SaltyEmu.Database;

namespace SaltyEmu.DatabasePlugin.Models.Character
{
    [Table("account")]
    public class AccountModel : IMappedModel
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

        [Required]
        public LanguageKey Language { get; set; }

        public List<CharacterModel> Characters { get; set; }
    }
}