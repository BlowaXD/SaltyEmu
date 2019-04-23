using Autofac.Extensions.DependencyInjection;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Helpers;
using Qmmands;
using SaltyEmu.Commands.Entities;
using SaltyEmu.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaltyEmu.Commands
{
    /* todo: find a better way to deal with TAP in world and here.
     *       handle errors correctly and return them to the user ingame.
     */
    public class CommandHandler : ICommandContainer
    {
        private readonly CommandService _commands;
        private readonly Logger _logger;

        public IServiceProvider Services { get; }

        /// <summary>
        ///     This class should be instanciated with our Container.
        /// </summary>
        public CommandHandler()
        {
            _logger = Logger.GetLogger<CommandHandler>();

            _commands = new CommandService(new CommandServiceConfiguration
            {
                CaseSensitive = false,
            });

            _commands.CommandExecuted += _commands_CommandExecuted;
            _commands.CommandErrored += _commands_CommandErrored;

            Services = new AutofacServiceProvider(ChickenContainer.Instance);
        }

        public async Task AddModuleAsync<T>() where T : SaltyModuleBase
        {
            await _commands.AddModuleAsync<T>();

            IReadOnlyList<Command> readOnlyList = _commands.GetAllModules().FirstOrDefault(s => s.Type == typeof(T))?.Commands;
            if (readOnlyList != null)
            {
                foreach (Command command in readOnlyList)
                {
                    _logger.Info($"[ADD_COMMAND] {command}");
                }
            }
        }

        public async Task RemoveModuleAsync<T>() where T : SaltyModuleBase
        {
            var module = _commands.GetAllModules().FirstOrDefault(s => s.Type == typeof(T));

            if (module is null)
            {
                throw new ArgumentException("The given module is not registered in the command container.");
            }

            await _commands.RemoveModuleAsync(module);
        }

        public Module[] GetModulesByName(string name, bool caseSensitive = true)
        {
            return _commands.GetAllModules().Where(x => caseSensitive ? x.Name == name : x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public Command[] GetCommandsByName(string name, bool caseSensitive = true)
        {
            return _commands.GetAllCommands().Where(x => caseSensitive ? x.Name == name : x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public Task RemoveCommandsAsync(string commandName, bool caseSensitive = true)
        {
            return RemoveCommandsAsync(GetCommandsByName(commandName, caseSensitive));
        }

        public async Task RemoveCommandsAsync(Command[] commands)
        {
            Task.WaitAll(commands.Select(RemoveCommandAsync).ToArray());
            foreach (Command command in commands)
            {
                await RemoveCommandAsync(command);
            }
        }

        public async Task RemoveCommandAsync(Command command)
        {
            var builder = new ModuleBuilder();

            var oldModule = command.Module;
            builder.AddAliases(oldModule.Aliases?.ToArray())
                .AddAttributes(oldModule.Attributes?.ToArray())
                .AddChecks(oldModule.Checks?.ToArray())
                .WithDescription(oldModule.Description)
                .WithIgnoreExtraArguments(oldModule.IgnoreExtraArguments)
                .WithName(oldModule.Name)
                .WithRemarks(oldModule.Remarks)
                .WithRunMode(oldModule.RunMode);

            foreach (var cmd in oldModule.Commands)
            {
                if (cmd.FullAliases[0] == command.FullAliases[0])
                {
                    continue;
                }

                var cmdBuilder = new CommandBuilder()
                    .AddAliases(cmd.Aliases?.ToArray())
                    .AddAttributes(cmd.Attributes?.ToArray())
                    .AddChecks(cmd.Checks?.ToArray())
                    .AddCooldowns(cmd.Cooldowns?.ToArray())
                    .WithCallback(cmd.Callback)
                    .WithDescription(cmd.Description)
                    .WithIgnoreExtraArguments(cmd.IgnoreExtraArguments)
                    .WithName(cmd.Name)
                    .WithPriority(cmd.Priority)
                    .WithRemarks(cmd.Remarks)
                    .WithRunMode(cmd.RunMode);

                foreach (var param in cmd.Parameters)
                {
                    var paramBuilder = new ParameterBuilder()
                        .AddAttributes(param.Attributes?.ToArray())
                        .AddChecks(param.Checks?.ToArray())
                        .WithCustomTypeParserType(param.CustomTypeParserType)
                        .WithDefaultValue(param.DefaultValue)
                        .WithDescription(param.Description)
                        .WithIsMultiple(param.IsMultiple)
                        .WithIsOptional(param.IsOptional)
                        .WithIsRemainder(param.IsRemainder)
                        .WithName(param.Name)
                        .WithRemarks(param.Remarks)
                        .WithType(param.Type);

                    cmdBuilder.AddParameter(paramBuilder);
                }

                builder.AddCommand(cmdBuilder);
            }

            await _commands.RemoveModuleAsync(oldModule);
            await _commands.AddModuleAsync(builder);
        }

        public void AddTypeParser<T>(TypeParser<T> typeParser)
        {
            _commands.AddTypeParser(typeParser);
            _logger.Info($"[ADD_TYPE_PARSER] {typeParser.GetType().Name}");
        }

        /// <summary>
        ///     This event is being invoked when the excecuted of a command threw an exception. 
        ///     Error results are handled by the result of CommandService#ExecuteAsync.
        /// </summary>
        /// <param name="result">Result with its associated exception.</param>
        /// <param name="context">It represents the context. Must be casted to our custom context (SaltyCommandContext)</param>
        /// <returns></returns>
        private Task _commands_CommandErrored(ExecutionFailedResult result, ICommandContext context, IServiceProvider services)
        {
            switch (result.Exception)
            {
                default:
                    _logger.Debug($"{result.Exception.GetType()} occured.\nError message: {result.Exception.Message}.\nStack trace: {result.Exception.StackTrace}");
                    break;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        ///     This event is being invoked when the execution of a command is over. When the command returned a result.
        ///     It could be a custom result that we can cast from our instance of CommandResult.
        /// </summary>
        /// <param name="command">It represents the command that has been executed.</param>
        /// <param name="result">It represents the returned result. It can an 'empty' result when the command returned a Task, or a custom result.</param>
        /// <param name="context">It represents the context. Must be casted to our custom context (SaltyCommandContext)</param>
        /// <returns></returns>
        private Task _commands_CommandExecuted(Command command, CommandResult result, ICommandContext context, IServiceProvider services)
        {
            var ctx = context as SaltyCommandContext;
            if (ctx is null)
            {
                _logger.Debug($"FairyMoveType context: {context.GetType()}. This is bad. Please report this.");
            }

            _logger.Debug($"The command {command.Name} (from player {ctx.Player.Character.Name} [{ctx.Player.Character.Id}]) has successfully been executed.");

            if (result is SaltyCommandResult res && !string.IsNullOrWhiteSpace(res.Message))
            {
                ctx.Player.SendChatMessageAsync(res.Message, SayColorType.Yellow);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        /// <summary>
        ///     This is where every message from the InGame tchat starting with our prefix will arrive. 
        ///     In our case, the parameter message represents the raw message sent by the user. 
        ///     The parameter of type object would represent the instance of the entity that invoked the command.
        ///     That method could be called on each messages sent in the in-game tchat. We will just check that it starts with our prefix ($).
        ///     Then we will create a Context that will propagate onto the command. 
        ///     The CommandService will try to parse the message and find a command with the parsed arguments and will perform some checks, if necessary.
        /// </summary>
        /// <param name="message">It represents the already parsed command with its parameters.</param>
        /// <param name="entity">It represents the instance of the entity that performed the action of sending a message.</param>
        public async Task HandleMessageAsync(string message, object entity)
        {
            if (!(entity is IPlayerEntity player))
            {
                return;
            }

            var ctx = new SaltyCommandContext(message, player, _commands);

            IResult result = await _commands.ExecuteAsync(ctx.Input, ctx, Services);

            if (result.IsSuccessful)
            {
                return;
            }

            await HandleErrorAsync(result, ctx);
        }

        /// <summary>
        ///     This is being called when the CommandService returned an unsuccessfull result.
        /// </summary>
        /// <param name="result">This represents the generic result returned by the command service. We'll check what was wrong.</param>
        /// <param name="ctx">This represents our context for this result.</param>
        private Task HandleErrorAsync(IResult result, SaltyCommandContext ctx)
        {
            _logger.Debug($"An error occured: {result}");

            var errorBuilder = new StringBuilder();
            var help = false;

            switch (result)
            {
                case ChecksFailedResult ex:
                    ctx.Command = ex.Command;
                    _logger.Debug("Some checks have failed: " + string.Join("\n", ex.FailedChecks.Select(x => x.Error)));
                    break;
                case TypeParseFailedResult ex:
                    errorBuilder.Append(ex.Reason);
                    ctx.Command = ex.Parameter.Command;
                    help = true;
                    break;
                case CommandNotFoundResult ex:
                    errorBuilder.Append($"The command was not found: {ctx.Input}");
                    break;
                case ArgumentParseFailedResult ex:
                    ctx.Command = ex.Command;
                    errorBuilder.Append($"The argument for the parameter {ex.Parameter.Name} was invalid.");
                    help = true;
                    break;
                case SaltyCommandResult ex:
                    errorBuilder.Append($"{ctx.Command.Name}: {ex.Message}");
                    break;
                case OverloadsFailedResult ex:
                    ctx.Command = ex.FailedOverloads.Select(x => x.Key).FirstOrDefault();
                    _logger.Debug($"Every overload failed: {string.Join("\n", ex.FailedOverloads.Select(x => x.Value.Reason))}");
                    errorBuilder.Append("Your command syntax was wrong.");
                    help = true;
                    break;
            }

            if (errorBuilder.Length == 0)
            {
                return Task.CompletedTask;
            }

            ctx.Player.SendChatMessageAsync(errorBuilder.ToString(), SayColorType.Green);

            return help
                ? _commands.ExecuteAsync($"help {ctx.Command.FullAliases[0]}", ctx)
                : Task.CompletedTask;
        }
    }
}