using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Game.Data.AccessLayer.Map;
using ChickenAPI.Game.Data.TransferObjects.Map;

namespace Toolkit.Converter
{
    public class MapDatConverter
    {
        private static readonly Logger Log = Logger.GetLogger<MapDatConverter>();
        private readonly List<MapDto> _maps = new List<MapDto>();
        private IMapService _mapService;

        public void Extract(string inputDirectory)
        {
            _mapService = ChickenContainer.Instance.Resolve<IMapService>();
            try
            {
                foreach (FileInfo file in new DirectoryInfo(inputDirectory).GetFiles().OrderBy(s => s.Name.Length).ThenBy(s => s.Name))
                {
                    try
                    {
                        byte[] data = File.ReadAllBytes(file.FullName);
                        var map = new MapDto
                        {
                            Name = file.Name,
                            Id = short.Parse(file.Name),
                            Width = BitConverter.ToInt16(data.AsSpan().Slice(0, 2).ToArray(), 0),
                            Height = BitConverter.ToInt16(data.AsSpan().Slice(2, 2).ToArray(), 0),
                            AllowPvp = false,
                            AllowShop = false,
                            Grid = data.AsSpan().Slice(4).ToArray()
                        };
                        _maps.Add(map);
                    }
                    catch (Exception e)
                    {
                        Log.Error("[ADD]", e);
                    }
                }

                _mapService.Save(_maps);
            }
            catch (Exception e)
            {
                Log.Error("[EXTRACT]", e);
            }
        }
    }
}