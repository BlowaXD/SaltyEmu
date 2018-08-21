using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Enums.Game.Portals;
using ChickenAPI.Game.Data.AccessLayer.Map;
using ChickenAPI.Game.Data.TransferObjects.Map;

namespace Toolkit.Generators.FromPackets
{
    public class PacketPortalGenerator
    {
        private static readonly Logger Log = Logger.GetLogger<PacketPortalGenerator>();

        private readonly List<PortalDto> _sourcePortals = new List<PortalDto>();
        private readonly List<PortalDto> _destinationPortals = new List<PortalDto>();

        public void Generate(string filePath)
        {
            var portalService = ChickenContainer.Instance.Resolve<IPortalService>();

            IEnumerable<PortalDto> portals = portalService.Get();
            IEnumerable<PortalDto> portalDtos = portals as PortalDto[] ?? portals.ToArray();
            _sourcePortals.AddRange(portalDtos);

            string[] lines = File.ReadAllText(filePath, Encoding.UTF8).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            short map = 0;

            foreach (string line in lines)
            {
                string[] currentPacket = line.Split('\t', ' ');
                switch (currentPacket[0])
                {
                    case "at":
                        map = short.Parse(currentPacket[2]);
                        break;

                    case "gp":
                        var portal = new PortalDto
                        {
                            SourceMapId = map,
                            SourceX = short.Parse(currentPacket[1]),
                            SourceY = short.Parse(currentPacket[2]),
                            DestinationMapId = short.Parse(currentPacket[3]),
                            Type = Enum.Parse<PortalType>(currentPacket[4]),
                            DestinationX = -1,
                            DestinationY = -1,
                            IsDisabled = false
                        };
                        if (_sourcePortals.Any(s => PortalsAreSame(s, portal)))
                        {
                            break;
                        }

                        _sourcePortals.Add(portal);
                        break;
                }
            }

            foreach (PortalDto pp in _sourcePortals)
            {
                PortalDto p = _sourcePortals.Except(_destinationPortals).FirstOrDefault(s => s.SourceMapId == pp.DestinationMapId && s.DestinationMapId == pp.SourceMapId);
                if (p == null)
                {
                    continue;
                }

                pp.DestinationX = p.SourceX;
                pp.DestinationY = p.SourceY;
                p.DestinationY = pp.SourceY;
                p.DestinationX = pp.SourceX;
                _destinationPortals.Add(p);
                _destinationPortals.Add(pp);
            }

            IEnumerable<PortalDto> portalsToSave = _destinationPortals.Where(
                    ppa => !portalService.GetByMapId(ppa.SourceMapId).Any(s => PortalsAreSame(s, ppa))).OrderBy(s => s.SourceMapId).ThenBy(s => s.DestinationMapId).ThenBy(s => s.SourceY)
                .ThenBy(s => s.SourceX).ToArray();

            portalService.Save(portalsToSave);
        }

        private static bool PortalsAreSame(PortalDto p, PortalDto pp) => 
            p.DestinationMapId == pp.DestinationMapId &&
            p.SourceMapId == pp.SourceMapId &&
            p.SourceX == pp.SourceX &&
            p.SourceY == pp.SourceY;
    }
}