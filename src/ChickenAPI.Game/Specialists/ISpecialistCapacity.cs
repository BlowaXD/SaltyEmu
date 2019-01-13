using ChickenAPI.Data.Item;

namespace ChickenAPI.Game.Specialists
{
    public interface ISpecialistCapacity : IMorphableCapacity
    {
        /// <summary>
        ///     Tells if the sp is in weared but not transformed
        /// </summary>
        bool HasSpWeared { get; }

        /// <summary>
        ///     Tells if the entity is transformed in its sp
        /// </summary>
        bool IsTransformedSp { get; }

        /// <summary>
        ///     Sp slot
        /// </summary>
        ItemInstanceDto Sp { get; }
    }
}