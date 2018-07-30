using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Player;

namespace ChickenAPI.Packets.Game.Server
{
    [PacketHeader("c_mode")]
    public class CModePacketBase : PacketBase
    {
        public CModePacketBase(IPlayerEntity entity)
        {
            var character = entity.GetComponent<CharacterComponent>();

            VisualType = VisualType.Character;
            CharacterId = character.Id;
            Morph = character.Morph;
            SpUpgrade = entity.GetComponent<SpecialistComponent>().Upgrade;
            SpDesign = entity.GetComponent<SpecialistComponent>().Design;
            ArenaWinner = character.ArenaWinner;
        }

        #region Properties

        [PacketIndex(0)]
        public VisualType VisualType { get; set; }

        [PacketIndex(1)]
        public long CharacterId { get; set; }

        [PacketIndex(2)]
        public short Morph { get; set; }

        [PacketIndex(3)]
        public byte SpUpgrade { get; set; }

        [PacketIndex(4)]
        public byte SpDesign { get; set; }

        [PacketIndex(5, IsOptional = true)]
        public bool ArenaWinner { get; set; }

        #endregion
    }
}
