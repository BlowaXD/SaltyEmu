using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Item;
using ChickenAPI.Enums;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Monster;
using ChickenAPI.Game.Inventory.Args;
using Qmmands;
using SaltyEmu.Commands.Checks;
using SaltyEmu.Commands.Entities;

namespace Essentials.Item
{
    [Group("Item")]
    [Description("It's a module related to butching monsters. It requires to be a GameMaster.")]
    [RequireAuthority(AuthorityType.GameMaster)]
    public class ItemModule : SaltyModuleBase
    {
        [Command("create")]
        [Description("Command that creates the item for you")]
        [Remarks("Only the itemId is needed")]
        public async Task<SaltyCommandResult> CreateItemAsync(long itemId, short quantity = 1)
        {
            var itemFactory = ChickenContainer.Instance.Resolve<IItemInstanceFactory>();
            ItemInstanceDto item = itemFactory.CreateItem(itemId, quantity);

            Player.EmitEvent(new InventoryAddItemEvent
            {
                ItemInstance = item
            });
            return await Task.FromResult(new SaltyCommandResult(true, $"Map have been cleaned"));
        }
    }
}