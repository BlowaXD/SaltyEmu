namespace ChickenAPI.Game.Locomotion
{
    public interface ILocomotionCapacity
    {
        /// <summary>
        /// </summary>
        bool IsTransformedLocomotion { get; }

        /// <summary>
        /// </summary>
        byte LocomotionSpeed { get; set; }
    }
}