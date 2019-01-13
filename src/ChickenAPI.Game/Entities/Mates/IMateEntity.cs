using ChickenAPI.Game.Battle.Interfaces;
using ChickenAPI.Game.Specialists;

namespace ChickenAPI.Game.Entities.Mates
{
    public interface IMateEntity : IBattleEntity, INpcMonsterEntity, ICharacterMateEntity, IMorphableEntity
    {
    }
}