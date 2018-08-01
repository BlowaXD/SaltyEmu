using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ChickenAPI.Core.Logging;

namespace Toolkit.Cleaners
{
    public class InPacketCleaner
    {
        private readonly List<string> _packetList = new List<string>();
        private static readonly Logger Log = Logger.GetLogger<InPacketCleaner>();

        public void Filter(string filePath, string outputPath)
        {

            string path = filePath;
            string tmp = File.ReadAllText(path, Encoding.GetEncoding(1252));
            string[] lines = tmp.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            HashSet<long> moverIds = new HashSet<long>();

            foreach (string line in lines.Where(s => s.StartsWith("at") || s.StartsWith("in 2") || s.StartsWith("in 3") || s.StartsWith("eff") || s.StartsWith("mv")))
            {
                if (line.StartsWith("mv"))
                {
                    string[] currentPacket = line.Split('\t', ' ');
                    long moverId = long.Parse(currentPacket[2]);
                    if (moverIds.Contains(moverId))
                    {
                        continue;
                    }
                    
                    moverIds.Add(moverId);
                }

                _packetList.Add(line);
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