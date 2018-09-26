using System.Collections.Generic;
using ChickenAPI.Core.ECS.Entities;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Features.NpcDialog.Handlers;
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
        bool HasPermission(PermissionsRequirementsAttribute permissions);

        long LastPulse { get; }

        void Broadcast<T>(T packet) where T : IPacket;
        void Broadcast<T>(IEnumerable<T> packets) where T : IPacket;

        void Broadcast<T>(T packet, bool doNotReceive) where T : IPacket;
        void Broadcast<T>(IEnumerable<T> packets, bool doNotReceive) where T : IPacket;

        void SendPacket<T>(T packetBase) where T : IPacket;

        void SendPackets<T>(IEnumerable<T> packets) where T : IPacket;

        void SendPackets(IEnumerable<IPacket> packets);
        void Save();
    }
}