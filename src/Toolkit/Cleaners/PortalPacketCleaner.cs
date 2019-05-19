using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ChickenAPI.Core.Logging;

namespace Toolkit.Cleaners
{
    public class PortalPacketCleaner
    {
        private readonly Queue<string> _packetList = new Queue<string>();

        public void Filter(string filePath, string outputPath)
        {
            string path = filePath;
            string tmp = File.ReadAllText(path, Encoding.GetEncoding(1252));
            string[] lines = tmp.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (!line.StartsWith("gp") || !line.StartsWith("at"))
                {
                    continue;
                }

                _packetList.Enqueue(line);
            }

            var builder = new StringBuilder();
            while (_packetList.TryDequeue(out string packet))
            {
                builder.AppendLine(packet);
            }

            builder.Replace('\t', ' ');

            File.WriteAllText(outputPath, builder.ToString(), Encoding.UTF8);
        }
    }
}