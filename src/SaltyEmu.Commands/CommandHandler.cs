using System;
using System.Linq;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Entities.Player;
using Qmmands;

namespace SaltyEmu.Commands
{
    public class CommandHandler
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
                CaseSensitive = true
            });

            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        ///     This method fetch for every command and/or module in our entry assembly.
        /// </summary>
        /// <remarks>
        ///     The commands and modules must be public and must inherit from an inheritance of ModuleBase.
        /// </remarks>
        public async Task InitializeAsync()
        {
            await _commands.AddModuleAsync<ItemModule>();

            _commands.CommandExecuted += _commands_CommandExecuted;
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
        private Task _commands_CommandExecuted(Command arg1, CommandResult arg2, ICommandContext arg3, IServiceProvider arg4)
        {
            var ctx = arg3 as SaltyCommandContext;

            _logger.Debug($"The command {arg1.Name} (from player {ctx.Sender.Character.Name} ({ctx.Sender.Character.Id}) has successfully been executed.");

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

            var pos = message.IndexOf('$') + 2;
            var command = message.Substring(pos);

            IResult result = await _commands.ExecuteAsync(command, ctx);

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
                    ctx.Sender.SendPacket(ctx.Sender.GenerateMessage)
                    break;
            }

            return Task.CompletedTask;
        }
    }
}