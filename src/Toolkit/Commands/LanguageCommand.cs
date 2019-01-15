using System;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.i18n;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game._i18n;
using CommandLine;
using SaltyEmu.Core;
using SaltyEmu.RedisWrappers;

namespace Toolkit.Commands
{
    [Verb("langs", HelpText = "Input languages from langs/ to running GameLanguageService")]
    public class LanguageCommand
    {
        private static readonly Logger Log = Logger.GetLogger<LanguageCommand>();
        private static readonly IGameLanguageService _langs = new Lazy<IGameLanguageService>(ChickenContainer.Instance.Resolve<IGameLanguageService>).Value;

        [Value(0, HelpText = "fr, en, de, all")]
        public string GeneratorType { get; set; }

        [Option('i', "input", HelpText = "Input directory", Required = true)]
        public string Input { get; set; }

        private static void ReadFileAndFillService(string file, LanguageKey key)
        {
            int i = 0;
            using (var langFileStream = new StreamReader(file, key.GetEncoding()))
            {
                string line;
                while ((line = langFileStream.ReadLine()) != null)
                {
                    string[] linesave = line.Split('\t');
                    if (linesave.Length <= 1)
                    {
                        continue;
                    }

                    i++;
                    _langs.SetLanguage(linesave[0], linesave[1], key);
                    _langs.SetLanguage(linesave[1], linesave[0], key);
                }

                langFileStream.Close();
            }

            Log.Info($"{file} : {i} language keys updated");
        }

        public static int Handle(LanguageCommand command)
        {
            new RedisPlugin().OnLoad();
            new RedisPlugin().OnEnable();
            ChickenContainer.Initialize();
            foreach (object enume in Enum.GetValues(typeof(LanguageKey)))
            {
                var element = (LanguageKey)enume;

                if (command.GeneratorType != "all" && command.GeneratorType != element.GetKeyForLangFiles())
                {
                    continue;
                }

                string subDirectory = command.Input + Path.DirectorySeparatorChar + element.GetKeyForLangFiles();
                if (!Directory.Exists(subDirectory))
                {
                    Directory.CreateDirectory(subDirectory);
                    Log.Warn($"Creating directory {subDirectory} for future lang files");
                    File.Create(subDirectory + Path.DirectorySeparatorChar + ".gitkeep");
                    continue;
                }

                Log.Info($"Parsing langugage files for {element.GetKeyForLangFiles()}");
                foreach (FileInfo filePath in new DirectoryInfo(subDirectory).GetFiles().Where(s => IsNostaleLangFile(s.Name, element)))
                {
                    Log.Info($"Parsing langugage file {filePath.Name}");
                    ReadFileAndFillService(filePath.FullName, element);
                }
            }

            return 0;
        }

        private static bool IsNostaleLangFile(string file, LanguageKey element)
        {
            string[] files =
            {
                "Card.txt",
                "MapIDData.txt",
                "MapPointData.txt",
                "Monster.txt",
                "Skill.txt",
                "Item.txt",
                "BCard.txt",
                "npctalk.txt",
                "Quest.txt"
            };
            return files.Any(s => file.Equals($"_code_{element.GetKeyForLangFiles()}_{s}", StringComparison.OrdinalIgnoreCase));
        }
    }
}