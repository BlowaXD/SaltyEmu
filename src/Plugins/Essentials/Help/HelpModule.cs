using ChickenAPI.Enums.Packets;
using Qmmands;
using SaltyEmu.Commands.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChickenAPI.Game.Helpers;

namespace Essentials.Help
{
    [Name("Help")] //todo: replace skip/take by proper 48 char limit paginator
    [Description("Module related to help.")]
    public class HelpModule : SaltyModuleBase
    {
        [Command("Help")]
        public async Task HelpAsync()
        {
            var modules = Context.CommandService.GetAllModules().Where(x => x.Aliases.Count > 0 && x.Commands.Count > 0 && x.Parent == null).ToList();
            var modulelessCommands = Context.CommandService.GetAllModules().Where(x => x.Aliases.Count == 0).SelectMany(x => x.Commands);

            foreach (var module in modules.ToList())
            {
                var result = await module.RunChecksAsync(Context);
                if (!result.IsSuccessful)
                {
                    modules.Remove(module);
                }
            }

            var commands = new List<Command>();
            foreach (var command in modulelessCommands.ToList())
            {
                if (commands.Any(x => x.FullAliases[0] == command.FullAliases[0]))
                {
                    continue;
                }

                var result = await command.RunChecksAsync(Context);
                if (result.IsSuccessful)
                {
                    commands.Add(command);
                }
            }

            var str = new StringBuilder();
            
            await Context.Player.SendChatMessageAsync("Help Command.", SayColorType.Purple);
            await Context.Player.SendChatMessageAsync("Modules available:", SayColorType.Purple);
            for (int i = 0; i < modules.Count / 6 + 1; i++)
            {
                await Context.Player.SendChatMessageAsync(" -> " + string.Join(", ", modules.Skip(i * 6).Take(6).Select(x => x.Name)), SayColorType.Green);
            }

            await Context.Player.SendChatMessageAsync("Commands available:", SayColorType.Purple);
            for (int i = 0; i < (commands.Count / 6) + 1; i++)
            {
                await Context.Player.SendChatMessageAsync(" -> " + string.Join(", ", commands.Skip(i * 6).Take(6).Select(x => x.Name)), SayColorType.Green);
            }
        }

        [Command("Help")]
        public async Task HelpAsync([Remainder] string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                await HelpAsync();
                return;
            }

            var cmds = Context.CommandService.FindCommands(command).ToList();

            foreach (var cmd in cmds.ToList())
            {
                var result = await cmd.Command.RunChecksAsync(Context);
                if (!result.IsSuccessful)
                {
                    cmds.Remove(cmd);
                }
            }

            if (cmds.Count == 0)
            {
                var module = Context.CommandService.FindModules(command).FirstOrDefault()?.Module;

                var passCheck = await module?.RunChecksAsync(Context);

                if (module is null || !passCheck.IsSuccessful)
                {
                    var cmdArgs = command.Split(' ').ToList();
                    cmdArgs.RemoveAt(cmdArgs.Count - 1);

                    await HelpAsync(string.Join(" ", cmdArgs));
                    return;
                }

                await Context.Player.SendChatMessageAsync($"Help: ({command})", SayColorType.Purple);
                if (module.Submodules.Count > 0)
                {
                    await Context.Player.SendChatMessageAsync("Submodules:", SayColorType.Purple);
                    for (int i = 0; i < (module.Submodules.Count / 6) + 1; i++)
                    {
                        await Context.Player.SendChatMessageAsync(" -> " + string.Join(", ", module.Submodules.Skip(i * 6).Take(6).Select(x => $"{x.Aliases[0]}")),
                            SayColorType.Green);
                    }
                }

                if (module.Commands.Count > 0)
                {
                    await Context.Player.SendChatMessageAsync("Commands:", SayColorType.Purple);
                    for (int i = 0; i < (module.Commands.Count / 6) + 1; i++)
                    {
                        await Context.Player.SendChatMessageAsync(" -> " + string.Join(", ", module.Commands.Skip(i * 6).Take(6).Select(x => $"{x.Aliases[0]}")), SayColorType.Green);
                    }
                }
            }

            await Context.Player.SendChatMessageAsync("Usages:", SayColorType.Purple);
            foreach (var cmd in cmds)
            {
                await Context.Player.SendChatMessageAsync(
                    $"${cmd.Command.Name} {string.Join(" ", cmd.Command.Parameters.Select(x => x.IsOptional ? $"<{x.Name}>" : $"[{x.Name}]"))}".ToLowerInvariant(), SayColorType.Green);
                foreach (var param in cmd.Command.Parameters)
                {
                    var str = "";

                    str = param.IsOptional ? $"<{param.Name}>:" : $"[{param.Name}]:";

                    str += $" {param.Description ?? "Undocumented yet."}";
                    await Context.Player.SendChatMessageAsync(str, SayColorType.Green);
                }

                await Context.Player.SendChatMessageAsync(cmd.Command.Description ?? "Undocumented yet.", SayColorType.Purple);
                await Context.Player.SendChatMessageAsync("-", SayColorType.Purple);
            }
        }
    }
}