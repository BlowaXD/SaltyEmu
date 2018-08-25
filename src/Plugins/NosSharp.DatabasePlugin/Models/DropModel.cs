using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;

namespace NosSharp.DatabasePlugin.Models
{
    [Table("global_drop")]
    public class DropModel : IMappedDto
    {
        /// <summary>
        ///     Item that will be dropped
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        ///     Amount of Item that have to be dropped
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        ///     Drop chance
        /// </summary>
        public int DropChance { get; set; }

        /// <summary>
        ///     Drop Id
        /// </summary>
        [Key]
        public long Id { get; set; }
    }
}