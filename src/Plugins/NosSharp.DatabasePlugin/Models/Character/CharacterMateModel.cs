using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data;
using SaltyEmu.Database;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;

namespace SaltyEmu.DatabasePlugin.Models.Character
{
    [Table("character_mate")]
    public class CharacterMateModel : IMappedModel
    {
        public CharacterModel Character { get; set; }

        [ForeignKey("FK_CHARACTERMATE_TO_CHARACTER")]
        public long CharacterId { get; set; }

        public byte Attack { get; set; }

        public bool CanPickUp { get; set; }

        public byte Defence { get; set; }

        public byte Direction { get; set; }

        public long Experience { get; set; }

        public int Hp { get; set; }

        public bool IsSummonable { get; set; }

        public bool IsTeamMember { get; set; }

        public byte Level { get; set; }

        public short Loyalty { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public int Mp { get; set; }

        public string Name { get; set; }

        public NpcMonsterModel NpcMonster { get; set; }

        [ForeignKey(nameof(NpcMonsterId))]
        public long NpcMonsterId { get; set; }

        public short Skin { get; set; }

        [Key]
        public long Id { get; set; }
    }
}