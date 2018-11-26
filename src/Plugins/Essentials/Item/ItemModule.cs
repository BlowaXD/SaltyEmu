using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Inventory.Args;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;

namespace Essentials.Item
{
    [Group("Item")]
    [Description("Module related to butching monsters. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    public class ItemModule : SaltyModuleBase
    {
        [Command("Create")]
        [Description("Command that creates an item for you.")]
        [Remarks("Quantity argument is facultative.")]
        public async Task<SaltyCommandResult> ItemCreateAsync(ItemDto item, short quantity = 1)
        {
            //todo: add a TypeParser for ItemInstanceDto.
            var itemFactory = ChickenContainer.Instance.Resolve<IItemInstanceFactory>();
            ItemInstanceDto itemInstance = itemFactory.CreateItem(item, quantity);

            Context.Player.EmitEvent(new InventoryAddItemEvent
            {
                ItemInstance = itemInstance
            });

            return await Task.FromResult(new SaltyCommandResult(true, "Your item(s) have been created."));
        }
    }
}