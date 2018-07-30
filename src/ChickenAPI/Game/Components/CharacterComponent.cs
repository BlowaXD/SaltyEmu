using ChickenAPI.Data.TransferObjects.Character;
using ChickenAPI.ECS.Components;
using ChickenAPI.ECS.Entities;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Game.Components
{
    public class CharacterComponent : IComponent
    {
        public CharacterComponent(IEntity entity) => Entity = entity;

        public CharacterComponent(IEntity entity, CharacterDto dto)
        {
            Entity = entity;

            Id = dto.Id;
            Authority = ((IPlayerEntity)entity).Session.Account.Authority;
            ArenaWinner = dto.ArenaWinner;
            Class = dto.Class;
            MapId = dto.MapId;
            Compliment = dto.Compliment;
            Gender = dto.Gender;
            HairColor = dto.HairColor;
            HairStyle = dto.HairStyle;
            ReputIcon = ReputationIconType.Beginner;
            Reputation = dto.Reput;
            Slot = dto.Slot;
            Dignity = (short)dto.Dignity;
        }

        public short Compliment { get; set; }

        public short Dignity { get; set; }

        public ReputationIconType ReputIcon { get; set; }

        public long Reputation { get; set; }

        public AuthorityType Authority { get; set; }

        public long Id { get; set; }

        public short MapId { get; set; }

        public byte Slot { get; set; }

        public CharacterClassType Class { get; set; }

        public GenderType Gender { get; set; }

        public HairColorType HairColor { get; set; }

        public HairStyleType HairStyle { get; set; }

        public bool ArenaWinner { get; set; }

        public short Morph { get; set; }

        public IEntity Entity { get; }

        public short Cp { get; set; }
    }
}