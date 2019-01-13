namespace ChickenAPI.Game.Specialists.Extensions
{
    public static class SpecialistExtensions
    {
        public static void WearSp(ISpecialistEntity entity)
        {
        }

        public static void SetMorph(this IMorphableEntity entity, short id)
        {
            entity.MorphId = id;
        }
    }
}