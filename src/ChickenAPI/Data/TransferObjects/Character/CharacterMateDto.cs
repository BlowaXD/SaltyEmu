﻿using ChickenAPI.Data.AccessLayer.Repository;

namespace ChickenAPI.Data.TransferObjects.Character
{
    public class CharacterMateDto : IMappedDto
    {
        public byte Attack { get; set; }

        public bool CanPickUp { get; set; }

        public long CharacterId { get; set; }

        public byte Defence { get; set; }

        public byte Direction { get; set; }

        public long Experience { get; set; }

        public int Hp { get; set; }

        public bool IsSummonable { get; set; }

        public bool IsTeamMember { get; set; }

        public byte Level { get; set; }

        public short Loyalty { get; set; }

        public short MapX { get; set; }

        public short MapY { get; set; }

        public int Mp { get; set; }

        public string Name { get; set; }

        public short VNum { get; set; }

        public short Skin { get; set; }
        public long Id { get; set; }
    }
}