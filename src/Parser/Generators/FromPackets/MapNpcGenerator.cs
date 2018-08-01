using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.AccessLayer.Map;
using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.Enums.Game.Entity;

namespace NosSharp.Parser.Generators.FromPackets
{
    public class MapNpcGenerator
    {
        private static readonly Logger Log = Logger.GetLogger<MapNpcGenerator>();

        private readonly List<MapNpcDto> _npcs = new List<MapNpcDto>();

        public void Generate(string filePath)
        {
            var mapNpcService = Container.Instance.Resolve<IMapNpcService>();
            Dictionary<long, short> effPacketsDictionary = new Dictionary<long, short>();
            List<long> npcMvPacketsList = new List<long>();
            List<long> mapNpcIds = new List<long>();
            _npcs.Add(new MapNpcDto
            {
                MapX = 102,
                MapY = 154,
                MapId = 5,
                NpcMonsterId = 860,
                Position = DirectionType.South,
                IsMoving = false,
                EffectDelay = 4750,
                Dialog = 999
            });

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
                        case "mv" when currentPacket[1].Equals("2") && !npcMvPacketsList.Contains(int.Parse(currentPacket[2])):
                            npcMvPacketsList.Add(int.Parse(currentPacket[2]));
                            break;

                        case "eff" when currentPacket[1].Equals("2") && !effPacketsDictionary.ContainsKey(int.Parse(currentPacket[2])):
                            effPacketsDictionary.Add(int.Parse(currentPacket[2]), short.Parse(currentPacket[3]));
                            break;

                        case "at":
                            map = short.Parse(currentPacket[2]);
                            break;

                        case "in" when currentPacket[1] == "2":
                            short npcId = short.Parse(currentPacket[2]);
                            long mapNpcId = long.Parse(currentPacket[3]);

                            if (long.Parse(currentPacket[3]) > 20000 || mapNpcService.GetById(mapNpcId) != null || mapNpcIds.Any(id => id == mapNpcId))
                            {
                                continue;
                            }

                            _npcs.Add(new MapNpcDto
                            {
                                MapX = short.Parse(currentPacket[4]),
                                MapY = short.Parse(currentPacket[5]),
                                MapId = map,
                                NpcMonsterId = npcId,
                                Id = mapNpcId,
                                EffectDelay = 4750,
                                IsMoving = npcMvPacketsList.Contains(long.Parse(currentPacket[3])),
                                Position = (DirectionType)byte.Parse(currentPacket[6]),
                                Dialog = short.Parse(currentPacket[9]),
                                IsSitting = currentPacket[13] != "1",
                                IsDisabled = false,
                                Effect = (short)(effPacketsDictionary.ContainsKey(mapNpcId) ? (npcId == 453 /*Lod*/ ? 855 : effPacketsDictionary[mapNpcId]) : 0)
                            });
                            mapNpcIds.Add(npcId);
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
            mapNpcService.Save(_npcs);
            Log.Info(string.Format("MAPNPC_PARSED", counter));
        }
    }
}
