using System;

namespace SaltyEmu.Database
{
    public interface ISynchronizedModel
    {
        Guid Id { get; set; }
    }
}