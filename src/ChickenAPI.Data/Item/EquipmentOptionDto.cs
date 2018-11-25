using System;

namespace ChickenAPI.Data.Item
{
    public class EquipmentOptionDto : ISynchronizedDto
    {
        public Guid Id { get; set; }

        public byte Level { get; set; }

        public byte Type { get; set; }

        public long Value { get; set; }

        public Guid WearableInstanceId { get; set; }
    }
}