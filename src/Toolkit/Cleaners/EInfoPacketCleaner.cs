using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Toolkit.Cleaners
{
    public class EInfoPacketCleaner
    {
        private readonly List<string> _packetList = new List<string>();

        public void Filter(string filePath, string outputPath)
        {
            string path = filePath;
            string tmp = File.ReadAllText(path, Encoding.GetEncoding(1252));
            string[] lines = tmp.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line.StartsWith("e_info 10"))
                {
                    _packetList.Add(line);
                }
            }

            var builder = new StringBuilder();
            foreach (string packet in _packetList)
            {
                builder.AppendLine(packet);
            }

            builder.Replace('\t', ' ');
            File.WriteAllText(outputPath, builder.ToString(), Encoding.UTF8);
        }
    }
}