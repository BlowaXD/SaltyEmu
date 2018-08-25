using System.Text;
using ChickenAPI.Core.Logging;
using CommandLine;
using Toolkit.Commands;

namespace Toolkit
{
    internal class CliProgram
    {
        private static readonly Logger Log = Logger.GetLogger<CliProgram>();

        private static int Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Logger.Initialize();
            return Parser.Default.ParseArguments<GenerateCommand, ParseCommand, CleanCommand>(args)
                .MapResult(
                    (GenerateCommand opts) => GenerateCommand.Handle(opts),
                    (ParseCommand opts) => ParseCommand.Handle(opts),
                    (CleanCommand opts) => CleanCommand.Handle(opts),
                    errs => 1);
        }
    }
}