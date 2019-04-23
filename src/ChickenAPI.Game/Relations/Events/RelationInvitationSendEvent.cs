using ChickenAPI.Enums.Game.Relations;
using ChickenAPI.Game._Events;
using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Game.Relations.Events
{
    public class RelationInvitationSendEvent : GameEntityEvent
    {
        public CharacterRelationType ExpectedRelationType { get; set; }
        public string TargetCharacterName { get; set; }
    }
}