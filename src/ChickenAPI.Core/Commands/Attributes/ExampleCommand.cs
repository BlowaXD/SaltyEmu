using System.ComponentModel;
using ChickenAPI.Core.Commands.Attributes;

// ReSharper disable once CheckNamespace
namespace ChickenAPI.Sample.Commands
{
    [CommandHeader("CreateItem")]
    [CommandDescription("This will create an item of the given Id and quantity")]
    public class ExampleCommand
    {
        [CommandArgumentIndex(0)]
        [CommandArgumentDescription("ItemId to generate")]
        [CommandArgumentRequired]
        public long ItemId { get; }

        [CommandArgumentIndex(1)]
        [CommandArgumentDescription("Quantity of the expected item")]
        [DefaultValue(1)]
        public long Quantity { get; }

        [CommandArgumentIndex(2)]
        [CommandArgumentDescription("Quantity of the expected item")]
        [DefaultValue(0)]
        public byte Rarity { get; }


        [CommandArgumentIndex(3)]
        [CommandArgumentDescription("Quantity of the expected item")]
        [DefaultValue(0)]
        public byte Upgrade { get; }
    }
}