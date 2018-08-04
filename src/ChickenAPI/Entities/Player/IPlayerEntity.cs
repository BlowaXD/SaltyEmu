using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Features.Visibility;
using ChickenAPI.Game.Game.Components;
using ChickenAPI.Game.Game.Network;
using ChickenAPI.Game.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public interface IPlayerEntity : IEntity, IMovableEntity, IBattleEntity, IInventoriedEntity, IExperenciedEntity, INamedEntity, IVisibleEntity
    {
        CharacterComponent Character { get; }
        ISession Session { get; }

        long LastPulse { get; }

        void SendPacket<T>(T packetBase) where T : IPacket;
        void SendPackets(IEnumerable<IPacket> packets);
    }
}