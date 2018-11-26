using System;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Player.Extension;
using Qmmands;
using SaltyEmu.Commands.Entities;
using SaltyEmu.Commands.Interfaces;

namespace SaltyEmu.Commands
{
    /* todo: find a better way to deal with TAP in world and here.
     *       handle errors correctly and return them to the user ingame.
     */
    public class CommandHandler : ICommandContainer
    {
        private readonly CommandService _commands;
        private readonly Logger _logger;

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

            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task AddModuleAsync<T>() where T : SaltyModuleBase
        {
            await _commands.AddModuleAsync<T>();
            foreach (Command command in _commands.GetAllModules().FirstOrDefault(s => s.Type == typeof(T)).Commands)
            {
                _logger.Info($"[ADD_COMMAND] {command}");
            }
        }

        /// <summary>
        ///     This method fetch for every command and/or module in our entry assembly.
        /// </summary>
        /// <remarks>
        ///     The commands and modules must be public and must inherit from an inheritance of ModuleBase.
        /// </remarks>
        public async Task InitializeAsync()
        {
            _commands.CommandExecuted += _commands_CommandExecuted;
            _commands.CommandErrored += _commands_CommandErrored;
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <returns></returns>
        private Task _commands_CommandErrored(ExecutionFailedResult arg1, ICommandContext arg2, IServiceProvider arg3)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        ///     This event is being invoked when the execution of a command is over. When the command returned a result.
        ///     It could be a custom result that we can cast from our instance of CommandResult.
        /// </summary>
        /// <param name="arg1">It represents the command that has been executed.</param>
        /// <param name="arg2">It represents the returned result. It can an 'empty' result when the command returned a Task, or a custom result.</param>
        /// <param name="arg3">It represents the context. Must be casted to our custom context (SaltyCommandContext)</param>
        /// <param name="arg4">It represents the Container (Usually Microsoft's Dependency Injection). Not used in our case.</param>
        /// <returns></returns>
        private Task _commands_CommandExecuted(Command command, CommandResult result, ICommandContext context, IServiceProvider services)
        {
            var ctx = context as SaltyCommandContext;

            _logger.Debug($"The command {command.Name} (from player {ctx.Sender.Character.Name} ({ctx.Sender.Character.Id}) has successfully been executed.");

            return Task.CompletedTask;
        }


        /// <summary>
        ///     This is a 'fake' example of a way to handle a message. In our case, the parameter message represents the raw message sent by the user. 
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

            var ctx = new SaltyCommandContext(message, player);

            IResult result = await _commands.ExecuteAsync(ctx.Input, ctx);

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
        /// <param name="context">This represents our context for this result.</param>
        private Task HandleErrorAsync(IResult result, SaltyCommandContext ctx)
        {
            _logger.Debug($"An error occured: {result}");

            switch (result)
            {
                case ChecksFailedResult ex:
                    _logger.Debug(string.Join("\n", ex.FailedChecks.Select(x => $"A check has failed with this reason: {x.Error}")));
                    break;
                case CommandNotFoundResult ex:
                    _logger.Debug("The command was not found. Raw input: " + ctx.Message);
                    ctx.Sender.SendPacket(ctx.Sender.GenerateSayPacket("The command was not found: " + ctx.Command.Name, SayColorType.Yellow));
                    break;
                case SaltyCommandResult ex:
                    ctx.Sender.SendPacket(ctx.Sender.GenerateSayPacket($"{ctx.Command.Name} : {ex.Message}", SayColorType.Green));
                    break;
            }

            return Task.CompletedTask;
        }
    }
}