using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Map;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Game.Data.AccessLayer.Map;

namespace Toolkit.Generators.FromPackets
{
    public class MapNpcGenerator
    {
        private static readonly Logger Log = Logger.GetLogger<MapNpcGenerator>();

        private readonly List<MapNpcDto> _npcs = new List<MapNpcDto>();

        public void Generate(string filePath)
        {
            var mapNpcService = ChickenContainer.Instance.Resolve<IMapNpcService>();
            Dictionary<long, MapNpcDto> npcs = mapNpcService.Get().ToDictionary(dto => dto.Id, dto => dto);
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

            foreach (string line in lines.Where(s => s.StartsWith("eff")))
            {
                try
                {
                    string[] currentPacket = line.Split('\t', ' ');
                    switch (currentPacket[0])
                    {
                        case "eff" when currentPacket[1].Equals("2") && !effPacketsDictionary.ContainsKey(int.Parse(currentPacket[2])):
                            effPacketsDictionary.Add(int.Parse(currentPacket[2]), short.Parse(currentPacket[3]));
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log.Error("[EFF]", e);
                    Log.Warn(line);
                }
            }

            foreach (string line in lines.Where(s => s.StartsWith("in") || s.StartsWith("mv") || s.StartsWith("eff") || s.StartsWith("at")))
            {
                try
                {
                    string[] currentPacket = line.Split('\t', ' ');
                    switch (currentPacket[0])
                    {
                        case "mv" when currentPacket[1].Equals("2") && !npcMvPacketsList.Contains(long.Parse(currentPacket[2])):
                            npcMvPacketsList.Add(long.Parse(currentPacket[2]));
                            break;


                        case "at":
                            map = short.Parse(currentPacket[2]);
                            break;

                        case "in" when currentPacket[1] == "2":
                            short npcId = short.Parse(currentPacket[2]);
                            long mapNpcId = long.Parse(currentPacket[3]);

                            if (mapNpcId > 20000 || npcs.ContainsKey(mapNpcId) || mapNpcIds.Any(id => id == mapNpcId))
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
                                IsMoving = npcMvPacketsList.Contains(mapNpcId),
                                Position = (DirectionType)byte.Parse(currentPacket[6]),
                                Dialog = short.Parse(currentPacket[9]),
                                IsSitting = currentPacket[13] != "1",
                                IsDisabled = false,
                                Effect = (short)(effPacketsDictionary.ContainsKey(mapNpcId) ? (npcId == 453 /*Lod*/ ? 855 : effPacketsDictionary[mapNpcId]) : 0)
                            });
                            mapNpcIds.Add(mapNpcId);
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
        }
    }
}