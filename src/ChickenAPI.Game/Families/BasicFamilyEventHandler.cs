using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.AccessLayer.Families;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Game.Managers;

namespace ChickenAPI.Game.Families
{
    public class BasicFamilyEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<BasicFamilyEventHandler>();
        private static readonly IPlayerManager PlayerManager = new Lazy<IPlayerManager>(() => ChickenContainer.Instance.Resolve<IPlayerManager>()).Value;
        private static readonly IFamilyService FamilyService = new Lazy<IFamilyService>(() => ChickenContainer.Instance.Resolve<IFamilyService>()).Value;
        private static readonly ICharacterFamilyService CharacterFamilyService = new Lazy<ICharacterFamilyService>(() => ChickenContainer.Instance.Resolve<ICharacterFamilyService>()).Value;

        public override ISet<Type> HandledTypes => new HashSet<Type>
        {
            typeof(FamilyCreationEvent)
        };

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case FamilyCreationEvent creation:
                    Task.WaitAll(CreateFamilyEvent(creation));
                    break;
                case FamilyJoinEvent join:
                    FamilyJoin(join);
                    break;
                case FamilyLeaveEvent leave:
                    FamilyLeave(leave);
                    break;
                case FamilyKickEvent kick:
                    FamilyKick(kick);
                    break;
            }
        }

        private void FamilyKick(FamilyKickEvent kick)
        {
            IPlayerEntity player = PlayerManager.GetPlayerByCharacterName(kick.CharacterName);
            if (player == null)
            {
                FamilyKickOffline(kick);
                return;
            }

            DetachFamily(player);
            player.Broadcast(player.GenerateGidxPacket());
        }

        private void FamilyKickOffline(FamilyKickEvent kick)
        {
            throw new NotImplementedException();
        }

        private void FamilyLeave(FamilyLeaveEvent leave)
        {
            if (leave.Player.IsFamilyLeader)
            {
                Log.Warn("CANT_LEAVE_FAMILY_LEADER");
                return;
            }
            DetachFamily(leave.Player);
            leave.Player.Broadcast(leave.Player.GenerateGidxPacket());
        }

        private static void FamilyJoin(FamilyJoinEvent join)
        {
            if (join.Player.HasFamily)
            {
                Log.Info("[FAMILY][JOIN] ALREADY_IN_FAMILY");
                return;
            }

            if (join.ExpectedAuthority == FamilyAuthority.Head)
            {
                Log.Info("[FAMILY][JOIN] CANT_HAVE_TWO_LEADERS");
                return;
            }

            AttachFamily(join.Player, join.Family, join.ExpectedAuthority);
            join.Player.Broadcast(join.Player.GenerateGidxPacket());
            join.Player.SendPacket(join.Player.GenerateGInfoPacket());
        }

        private async Task CreateFamilyEvent(FamilyCreationEvent creation)
        {
            if (FamilyService.GetByName(creation.FamilyName) != null)
            {
                Log.Info("[FAMILY][CREATION] NAME_ALREADY_TAKEN");
                return;
            }

            if (creation.Leader.HasFamily)
            {
                Log.Info("[FAMILY][CREATION] ALREADY_IN_FAMILY");
                return;
            }

            var family = new FamilyDto
            {
                Name = creation.FamilyName,
                FamilyFaction = FactionType.Angel,
                FamilyHeadGender = creation.Leader.Character.Gender,
                FamilyMessage = string.Empty,
                MaxSize = 50,
            };
            family = await FamilyService.SaveAsync(family);
            // todo family object shared across all entities
            AttachFamily(creation.Leader, family, FamilyAuthority.Head);
            creation.Leader.Broadcast(creation.Leader.GenerateGidxPacket());
            creation.Leader.SendPacket(creation.Leader.GenerateGInfoPacket());

            if (creation.Assistants == null)
            {
                return;
            }

            foreach (IPlayerEntity player in creation.Assistants)
            {
                if (player.HasFamily)
                {
                    continue;
                }

                AttachFamily(player, family, FamilyAuthority.Assistant);
                player.Broadcast(player.GenerateGidxPacket());
                player.SendPacket(player.GenerateGInfoPacket());
            }
        }

        private static void DetachFamily(IPlayerEntity player)
        {
            CharacterFamilyService.DeleteById(player.FamilyCharacter.Id);
            player.FamilyCharacter = null;
            player.Family = null;
            // globally update family
        }

        private static void AttachFamily(IPlayerEntity player, FamilyDto dto, FamilyAuthority authority)
        {
            if (dto == null)
            {
                return;
            }

            player.Family = dto;
            player.FamilyCharacter = CharacterFamilyService.Save(new CharacterFamilyDto
            {
                Authority = authority,
                CharacterId = player.Character.Id,
                FamilyId = dto.Id,
                Rank = 0
            });
        }
    }
}