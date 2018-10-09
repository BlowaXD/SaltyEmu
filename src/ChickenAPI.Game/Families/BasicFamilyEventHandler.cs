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

namespace ChickenAPI.Game.Families
{
    public class BasicFamilyEventHandler : EventHandlerBase
    {
        private static readonly Logger Log = Logger.GetLogger<BasicFamilyEventHandler>();
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
                    CreateFamilyEvent(creation).Wait();
                    break;
                case FamilyJoinEvent join:
                    FamilyJoin(join);
                    break;
                case FamilyLeaveEvent leave:
                    FamilyLeave(leave);
                    break;
            }
        }

        private void FamilyLeave(FamilyLeaveEvent leave)
        {
            // todo
        }

        private void FamilyJoin(FamilyJoinEvent join)
        {
            // todo
        }

        private async Task CreateFamilyEvent(FamilyCreationEvent creation)
        {
            if (FamilyService.GetByName(creation.FamilyName) != null)
            {
                Log.Info("[FAMILY][CREATION] NAME_ALREADY_TAKEN");
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
            await FamilyService.SaveAsync(family);
            // todo family object shared across all entities

            AttachFamily(creation.Leader, family, FamilyAuthority.Head);

            creation.Leader.Broadcast(creation.Leader.GenerateGidxPacket());
            creation.Leader.Broadcast(creation.Leader.GenerateGInfoPacket());

            if (creation.Assistants == null)
            {
                return;
            }

            foreach (IPlayerEntity player in creation.Assistants)
            {
                AttachFamily(player, family, FamilyAuthority.Assistant);
                player.Broadcast(creation.Leader.GenerateGidxPacket());
                player.Broadcast(creation.Leader.GenerateGInfoPacket());
            }

        }

        private static void AttachFamily(IPlayerEntity player, FamilyDto dto, FamilyAuthority authority)
        {
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