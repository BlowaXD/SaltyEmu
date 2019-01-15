using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Managers;
using Qmmands;
using System;
using System.Threading.Tasks;
using ChickenAPI.Game._ECS.Entities;

namespace SaltyEmu.Commands.TypeParsers
{
    public sealed class MapLayerTypeParser : TypeParser<IMapLayer>
    {
        public override Task<TypeParserResult<IMapLayer>> ParseAsync(string value, ICommandContext context, IServiceProvider provider)
        {
            if (!long.TryParse(value, out var result))
            {
                return Task.FromResult(new TypeParserResult<IMapLayer>($"The given map ID was invalid. ({value})"));
            }

            var manager = ChickenContainer.Instance.Resolve<IMapManager>();
            IMapLayer map = manager.GetBaseMapLayer(result);

            return map is null
                ? Task.FromResult(new TypeParserResult<IMapLayer>($"A map with ID#{result} doesn't exist."))
                : Task.FromResult(new TypeParserResult<IMapLayer>(map));
        }
    }
}
