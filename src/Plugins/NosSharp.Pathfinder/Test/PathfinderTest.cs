using System;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Maps;
using NosSharp.Pathfinder.Pathfinder;

namespace NosSharp.Pathfinder.Test
{
    public class PathfinderTest
    {
        private static readonly Logger Log = Logger.GetLogger<PathfinderTest>();

        public void RunTest(int repetition)
        {
            var pathfinder = ChickenContainer.Instance.Resolve<IPathfinder>();
            var map = new SimpleMap(new ChickenAPI.Game.Data.TransferObjects.Map.MapDto()
            {
                Height = 5,
                Width = 5,
                Grid = new byte[]
                {
                    0,0,0,0,0,
                    0,0,0,0,0,
                    0,0,0,0,0,
                    0,0,0,0,0,
                    0,0,0,0,0
                }
            }, null, null, null, null);

            var path = new Position<short>[0];

            DateTime t = DateTime.Now;
            for (int a = 0; a < repetition; a++)
            {
                path = pathfinder.FindPath(new Position<short> { X = 0, Y = 0 }, new Position<short> { X = 4, Y = 4 }, map);
            }
            Log.Info($"Time used for {repetition} repetition : {DateTime.Now - t}");
            Log.Info("Path :");
            foreach (var pos in path)
            {
                Log.Info($"x : {pos.X}, y : {pos.Y}");
            }
        }
    }
}
