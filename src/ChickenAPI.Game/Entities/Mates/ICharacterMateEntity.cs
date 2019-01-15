using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Entities.Mates
{
    public interface ICharacterMateEntity
    {
        CharacterMateDto Mate { get; }

        byte PetId { get; set; }

        IPlayerEntity Owner { get; set; }
    }
}