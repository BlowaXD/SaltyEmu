using System.ComponentModel.DataAnnotations.Schema;
using ChickenAPI.Core.Data.TransferObjects;
using ChickenAPI.Data.AccessLayer.Repository;

namespace NosSharp.DatabasePlugin.Models.BCard
{
    [Table("global_bcard")]
    public class BCardModel : IMappedDto
    {
        public long Id { get; set; }

        public byte SubType { get; set; }

        public byte Type { get; set; }

        public int FirstData { get; set; }

        public int SecondData { get; set; }

        public int ThirdData { get; set; }

        public byte CastType { get; set; }

        public bool IsLevelScaled { get; set; }

        public bool IsLevelDivided { get; set; }
    }
}