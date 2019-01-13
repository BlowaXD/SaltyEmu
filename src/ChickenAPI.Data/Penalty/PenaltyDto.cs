using System;

namespace ChickenAPI.Data.Penalty
{
    public class PenaltyDto : ISynchronizedDto
    {
        public PenaltyType Type { get; set; }

        public long TargetedCharacterId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Reason { get; set; }


        /// <summary>
        /// </summary>
        public long SenderCharacterId { get; set; }

        public Guid Id { get; set; }
    }
}