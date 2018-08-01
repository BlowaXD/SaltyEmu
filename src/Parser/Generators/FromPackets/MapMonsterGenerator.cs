using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.AccessLayer.Map;
using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.Enums.Game.Entity;

namespace Toolkit.Generators.FromPackets
{
    public class MapMonsterGenerator
    {
        private static readonly Logger Log = Logger.GetLogger<MapMonsterGenerator>();

        private readonly List<MapMonsterDto> _monsters = new List<MapMonsterDto>();

        public void Generate(string filePath)
        {
            var mapMonsterService = Container.Instance.Resolve<IMapMonsterService>();
            Dictionary<long, short> effPacketsDictionary = new Dictionary<long, short>();
            List<long> npcMvPacketsList = new List<long>();
            List<long> mapMonsterIds = new List<long>();

            string[] lines = File.ReadAllText(filePath, Encoding.UTF8).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int counter = 0;
            short map = 0;

            foreach (string line in lines.Where(s => s.StartsWith("in") || s.StartsWith("mv") || s.StartsWith("eff") || s.StartsWith("at")))
            {
                try
                {
                    string[] currentPacket = line.Split('\t', ' ');
                    switch (currentPacket[0])
                    {
                        case "mv" when currentPacket[1].Equals("3") && !npcMvPacketsList.Contains(int.Parse(currentPacket[2])):
                            npcMvPacketsList.Add(int.Parse(currentPacket[2]));
                            break;

                        case "eff" when currentPacket[1].Equals("3") && !effPacketsDictionary.ContainsKey(int.Parse(currentPacket[2])):
                            effPacketsDictionary.Add(int.Parse(currentPacket[2]), short.Parse(currentPacket[3]));
                            break;

                        case "at":
                            map = short.Parse(currentPacket[2]);
                            break;

                        case "in" when currentPacket[1] == "3" && mapMonsterService.GetById(short.Parse(currentPacket[2])) == null && mapMonsterIds.All(id => id != long.Parse(currentPacket[3])):
                            _monsters.Add(new MapMonsterDto
                            {
                                MapX = short.Parse(currentPacket[4]),
                                MapY = short.Parse(currentPacket[5]),
                                MapId = map,
                                NpcMonsterId = short.Parse(currentPacket[2]),
                                Id = long.Parse(currentPacket[3]),
                                IsMoving = npcMvPacketsList.Contains(long.Parse(currentPacket[3])),
                                Position = (DirectionType)byte.Parse(currentPacket[6]),
                                IsDisabled = false
                            });
                            mapMonsterIds.Add(long.Parse(currentPacket[3]));
                            counter++;
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error("[PARSE]", e);
                    Log.Warn(line);
                }
            }
            mapMonsterService.Save(_monsters);
            Log.Info(string.Format("MAPMONSTERS_PARSED", counter));
        }
    }
}
