using System;

namespace ChickenAPI.Game.Specialists
{
    public interface IMorphableCapacity
    {
        /// <summary>
        ///     If the entity is under morph, this is the morphId
        /// </summary>
        short MorphId { get; set; }

        /// <summary>
        ///     Date when the last morph has occured
        /// </summary>
        DateTime LastMorphUtc { get; set; }

        int SpCoolDown { get; set; }
    }
}