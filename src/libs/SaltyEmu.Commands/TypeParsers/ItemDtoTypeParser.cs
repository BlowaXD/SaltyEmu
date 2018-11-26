using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Item;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Managers;
using Qmmands;

namespace SaltyEmu.Commands.TypeParsers
{
    public sealed class ItemDtoTypeParser : TypeParser<ItemDto>
    {
        public override async Task<TypeParserResult<ItemDto>> ParseAsync(string value, ICommandContext context, IServiceProvider provider)
        {
            if (!long.TryParse(value, out long result))
            {
                return await Task.FromResult(new TypeParserResult<ItemDto>($"The given Item ID was invalid. ({value})"));
            }

            var manager = ChickenContainer.Instance.Resolve<IItemService>();
            ItemDto item = await manager.GetByIdAsync(result);

            return item is null
                ? await Task.FromResult(new TypeParserResult<ItemDto>($"There is no Item with ID#{result}"))
                : await Task.FromResult(new TypeParserResult<ItemDto>(item));
        }
    }
}