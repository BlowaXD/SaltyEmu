using ChickenAPI.Enums.Game.Drop;

namespace ChickenAPI.Data.Drop
{
    public class DropDto : IMappedDto
    {
        /// <summary>
        ///     Drop RelationType
        /// </summary>
        public DropType RelationType { get; set; }

        /// <summary>
        ///     This can be MapTypeId, NpcMonsterId...
        /// </summary>
        public long TypedId { get; set; }

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
        public long Id { get; set; }
    }
}