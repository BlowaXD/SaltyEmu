using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Features.Visibility;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Permissions;
using ChickenAPI.Packets;

namespace ChickenAPI.Game.Entities.Player
{
    public interface IPlayerEntity : IEntity, IMovableEntity, IBattleEntity, IInventoriedEntity, IExperenciedEntity, IVisibleEntity, ISkillEntity, ISpecialistEntity, IQuicklistEntity
    {
        CharacterDto Character { get; }
        ISession Session { get; }

        bool HasPermission(PermissionType permission);
        bool HasPermission(string permissionKey);

        long LastPulse { get; }

        void SendPacket<T>(T packetBase) where T : IPacket;

        void SendPackets<T>(IEnumerable<T> packets) where T : IPacket;

        void SendPackets(IEnumerable<IPacket> packets);
        void Save();
    }
}