using System;

namespace ChickenAPI.Data.Item
{
    public class EquipmentOptionDto : ISynchronizedDto
    {
        public byte Level { get; set; }

        public byte Type { get; set; }

        public long Value { get; set; }

        public Guid WearableInstanceId { get; set; }
        public Guid Id { get; set; }
    }
}