using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums;
using ChickenAPI.Game.Inventory.Events;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;

namespace Essentials.Item
{
    [Name("Item")]
    [Group("Item")]
    [Description("Module related to items. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    public class ItemModule : SaltyModuleBase
    {
        [Command("Create")]
        [Description("Command that creates an item for you.")]
        [Remarks("Quantity argument is facultative.")]
        public Task<SaltyCommandResult> ItemCreateAsync(ItemDto item, short quantity = 1)
        {
            var itemFactory = ChickenContainer.Instance.Resolve<IItemInstanceDtoFactory>();
            ItemInstanceDto itemInstance = itemFactory.CreateItem(item, quantity);

            Context.Player.EmitEvent(new InventoryAddItemEvent
            {
                ItemInstance = itemInstance
            });

            return Task.FromResult(new SaltyCommandResult(true, "Your item(s) have been created."));
        }
    }
}