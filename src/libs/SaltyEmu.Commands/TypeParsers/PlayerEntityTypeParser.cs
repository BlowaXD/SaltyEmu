using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Managers;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace SaltyEmu.Commands.TypeParsers
{
    public sealed class PlayerEntityTypeParser : TypeParser<IPlayerEntity>
    {
        public override Task<TypeParserResult<IPlayerEntity>> ParseAsync(string value, ICommandContext context, IServiceProvider provider)
        {
            var manager = ChickenContainer.Instance.Resolve<IPlayerManager>();
            IPlayerEntity player = manager.GetPlayerByCharacterName(value);

            return player is null
                ? Task.FromResult(new TypeParserResult<IPlayerEntity>($"{value} is not connected on your server"))
                : Task.FromResult(new TypeParserResult<IPlayerEntity>(player));
        }
    }
}
