using ChickenAPI.Game.Features.Specialists;

namespace ChickenAPI.Game.Entities
{
    public interface ISpecialistEntity
    {
        SpecialistComponent Sp { get; }
    }
}