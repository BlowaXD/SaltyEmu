namespace ChickenAPI.Game.Specialists
{
    public interface IMorphableCapacity
    {
        /// <summary>
        /// If the entity is under morph, this is the morphId
        /// </summary>
        long MorphId { get; }
    }
}