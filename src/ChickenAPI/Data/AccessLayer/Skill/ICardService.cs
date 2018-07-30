using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Data.TransferObjects.Skills;

namespace ChickenAPI.Data.AccessLayer.Skill
{
    public interface ICardService : IMappedRepository<CardDto>
    {
    }
}