using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Data.AccessLayer.Repository;
using ChickenAPI.Enums.Game.Portals;

namespace NosSharp.DatabasePlugin.Models.Map
{
    [Table("map_portals")]
    public class MapPortalModel : IMappedDto
    {
        public long Id { get; set; }


        public PortalType Type { get; set; }

        public long DestinationMapId { get; set; }

        public short DestinationX { get; set; }

        public short DestinationY { get; set; }

        public bool IsDisabled { get; set; }

        public MapModel SourceMap { get; set; }


        [ForeignKey("FK_MAPPORTAL_TO_SRC_MAP")]
        public long SourceMapId { get; set; }

        public short SourceX { get; set; }

        public short SourceY { get; set; }
    }
}