using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Core.Utils;
using ChickenAPI.Game.Data.TransferObjects.Map;

namespace ChickenAPI.Game.Maps
{
    public interface IMap : IEntityManager
    {
        /// <summary>
        ///     This is the mapId
        /// </summary>
        long Id { get; }

        /// <summary>
        ///     This is the music id on this map
        /// </summary>
        int MusicId { get; }

        /// <summary>
        ///     This layer is the base map where everyone will be by default
        /// </summary>
        IMapLayer BaseLayer { get; }

        /// <summary>
        ///     Different layers of the Map
        /// </summary>
        HashSet<IMapLayer> Layers { get; }

        /// <summary>
        ///     Map Width
        /// </summary>
        short Width { get; }

        /// <summary>
        ///     Map Height
        /// </summary>
        short Height { get; }

        /// <summary>
        ///     Get the Map Grid
        /// </summary>
        byte[] Grid { get; }

        /// <summary>
        ///     Get if the map is walkable
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool IsWalkable(short x, short y);

        Position<short> GetFreePosition(short minimumX, short minimumY, short maximumX, short maximumY);

        PortalDto GetPortalFromPosition(short x, short y, short range = 2);
    }
}