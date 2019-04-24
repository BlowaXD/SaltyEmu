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
        public override Task<TypeParserResult<IPlayerEntity>> ParseAsync(Parameter param, string value, ICommandContext context, IServiceProvider provider)
        {
            var manager = ChickenContainer.Instance.Resolve<IPlayerManager>();
            IPlayerEntity player = manager.GetPlayerByCharacterName(value);

            return player is null
                ? Task.FromResult(new TypeParserResult<IPlayerEntity>($"Player {value} is not connected or doesn't exist."))
                : Task.FromResult(new TypeParserResult<IPlayerEntity>(player));
        }
    }
}