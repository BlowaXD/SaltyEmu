﻿using ChickenAPI.Packets.Enumerations;

namespace ChickenAPI.Data.Map
{
    public class PortalDto : IMappedDto
    {
        public short DestinationMapId { get; set; }

        public short DestinationX { get; set; }

        public short DestinationY { get; set; }

        public bool IsDisabled { get; set; }

        public short SourceMapId { get; set; }

        public short SourceX { get; set; }

        public short SourceY { get; set; }

        public PortalType Type { get; set; }
        public long Id { get; set; }
    }
}