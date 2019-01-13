using System;
using System.Collections.Generic;
using ChickenAPI.Data.Item;

namespace ChickenAPI.Data.Gift
{
    public class GiftDto : ISynchronizedDto
    {
        public long TargetCharacterId { get; set; }

        public long SenderCharacterId { get; set; }
        public string Text { get; set; }
        public DateTime SentDate { get; set; }

        public IEnumerable<ItemInstanceDto> Gifts { get; set; }
        public Guid Id { get; set; }
    }
}