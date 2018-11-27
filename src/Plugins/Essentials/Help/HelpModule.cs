using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Player.Extension;
using Qmmands;
using SaltyEmu.Commands.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (commands.All(x => x.FullAliases[0] != command.FullAliases[0]))
                {
                    var result = await command.RunChecksAsync(Context);
                    if (result.IsSuccessful)
                    {
                        commands.Add(command);
                    }
                }
            }

            StringBuilder str = new StringBuilder();

            Context.Player.SendPacket(Context.Player.GenerateSayPacket("Help Command.", SayColorType.Yellow));
            Context.Player.SendPacket(Context.Player.GenerateSayPacket("Modules available:", SayColorType.Yellow));
            for (int i = 0; i < (modules.Count / 6) + 1; i++)
            {
                Context.Player.SendPacket(Context.Player.GenerateSayPacket(" -> " + string.Join(", ", modules.Skip(i * 6).Take(6).Select(x => x.Name)), SayColorType.Yellow));
            }

            Context.Player.SendPacket(Context.Player.GenerateSayPacket("Commands available:", SayColorType.Yellow));
            for (int i = 0; i < (modules.Count / 6) + 1; i++)
            {
                Context.Player.SendPacket(Context.Player.GenerateSayPacket(" -> " + string.Join(", ", commands.Skip(i * 6).Take(6).Select(x => x.Name)), SayColorType.Yellow));
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

                Context.Player.SendPacket(Context.Player.GenerateSayPacket($"Help: ({command})", SayColorType.Yellow));
                if (module.Submodules.Count > 0)
                {
                    Context.Player.SendPacket(Context.Player.GenerateSayPacket("Submodules:", SayColorType.Yellow));
                    for (int i = 0; i < (module.Submodules.Count / 6) + 1; i++)
                    {
                        Context.Player.SendPacket(Context.Player.GenerateSayPacket(" -> " + string.Join(", ", module.Submodules.Skip(i * 6).Take(6).Select(x => $"{x.Aliases[0]}")), SayColorType.Yellow));
                    }
                }

                if (module.Commands.Count > 0)
                {
                    Context.Player.SendPacket(Context.Player.GenerateSayPacket("Commands:", SayColorType.Yellow));
                    for (int i = 0; i < (module.Commands.Count / 6) + 1; i++)
                    {
                        Context.Player.SendPacket(Context.Player.GenerateSayPacket(" -> " + string.Join(", ", module.Commands.Skip(i * 6).Take(6).Select(x => $"{x.Aliases[0]}")), SayColorType.Yellow));
                    }
                }
            }

            Context.Player.SendPacket(Context.Player.GenerateSayPacket("Usages:", SayColorType.Yellow));
            foreach (var cmd in cmds)
            {
                Context.Player.SendPacket(Context.Player.GenerateSayPacket($"${cmd.Command.Name} {string.Join(" ", cmd.Command.Parameters.Select(x => x.IsOptional ? $"<{x.Name}>" : $"[{x.Name}]"))}".ToLowerInvariant(), SayColorType.Yellow));
                foreach (var param in cmd.Command.Parameters)
                {
                    var str = "";

                    if (param.IsOptional)
                    {
                        str = $"<{param.Name}>:";
                    }
                    else
                    {
                        str = $"[{param.Name}]:";
                    }

                    str += $" {param.Description ?? "Undocumented yet."}";

                    Context.Player.SendPacket(Context.Player.GenerateSayPacket(str, SayColorType.Yellow));
                }

                Context.Player.SendPacket(Context.Player.GenerateSayPacket(cmd.Command.Description ?? "Undocumented yet.", SayColorType.Yellow));
                Context.Player.SendPacket(Context.Player.GenerateSayPacket("-", SayColorType.Yellow));
            }
        }
    }
}
