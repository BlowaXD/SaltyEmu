using System.IO;
using ChickenAPI.Core.Logging;
using CommandLine;
using NosSharp.Parser.Cleaners;

namespace NosSharp.Parser.Commands
{
    [Verb("clean", HelpText = "Clean packets")]
    public class CleanCommand
    {
        [Value(0, HelpText = "e_info, in, portal, shop")]
        public string CleanType { get; set; }

        [Option('v', "verbose", Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('i', "input", HelpText = "Input directory", Required = true)]
        public string Input { get; set; }

        [Option('o', "output", HelpText = "Output directory", Required = true)]
        public string Output { get; set; }

        public static int Handle(CleanCommand opts)
        {
            if (!File.Exists(opts.Input))
            {
                return 1;
            }

            if (opts.Verbose)
            {
                Logger.Initialize();
            }

            switch (opts.CleanType)
            {
                case "e_info":
                    new EInfoPacketCleaner().Filter(opts.Input, opts.Output);
                    break;
                case "in":
                    new InPacketCleaner().Filter(opts.Input, opts.Output);
                    break;
            }
            return 0;
        }
    }
}