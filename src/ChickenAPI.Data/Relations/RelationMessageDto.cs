using System;

namespace ChickenAPI.Data.Relations
{
    public class RelationMessageDto : ISynchronizedDto
    {
        public long SenderId { get; set; }
        public long TargetId { get; set; }

        public string Message { get; set; }

        public DateTime SentDate { get; set; }
        public DateTime ReadDate { get; set; }
        public Guid Id { get; set; }
    }
}