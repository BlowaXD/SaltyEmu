using System;
using System.Threading.Tasks;
using Qmmands;

namespace SaltyEmu.Commands
{
    [Group("Item")]
    [Description("It's a module related to Item manipulation. It requires to be a GameMaster.")]
    [RequireGameMaster]
    public class ItemModule : SaltyModuleBase
    {
        [Command("create")]
        [Description("Command that tries to create an item by specifying its id and the actual quantity we need.")]
        [Remarks("The two parameters are needed.")]
        public Task CreateAsync([Description("ID of the item to create.")] int id, 
            [Description("Amount of the item to create.")] int quantity)
        {
            //do stuff
            Console.WriteLine($"Creating {quantity} item(s) with ID#{id} for the player {Context.Sender.Id}");

            return Task.CompletedTask;
        }

        [Command("delete")]
        [Description("Command that tries to delete an item by specifying its id and its position in the player's inventory.")]
        [Remarks("The two parameters are needed.")]
        public Task DeleteAsync([Description("ID of the item to delete.")] int id, 
            [Description("Position of the item in the player's inventory.")] int position)
        {
            //do stuff
            Console.WriteLine($"Deleting the item with ID#{id} for the entity {Context.Sender.Id} at the inventory position {position}");

            return Task.CompletedTask;
        }
    }
}