using System;

namespace SaltyEmu.DatabasePlugin.Models
{
    public interface ISynchronizedModel
    {
        Guid Id { get; set; }
    }
}