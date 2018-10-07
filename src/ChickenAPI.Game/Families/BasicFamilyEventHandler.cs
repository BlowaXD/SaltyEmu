using System;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Game.Data.AccessLayer.Families;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Families.Events;

namespace ChickenAPI.Game.Families
{
    public class BasicFamilyEventHandler : EventHandlerBase
    {
        private static readonly IFamilyService FamilyService = new Lazy<IFamilyService>(() => ChickenContainer.Instance.Resolve<IFamilyService>()).Value;

        public override void Execute(IEntity entity, ChickenEventArgs e)
        {
            switch (e)
            {
                case FamilyCreationEvent creation:
                    CreateFamilyEvent(creation).Wait();
                    break;
            }
        }

        private async Task CreateFamilyEvent(FamilyCreationEvent creation)
        {
            // check family name already here

            if (FamilyService.GetByName(creation.FamilyName) != null)
            {
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

            foreach (IPlayerEntity i in creation.Assistants)
            {
                AttachFamily(i, family, FamilyAuthority.Assistant);
            }

            // broadcast packets
        }

        private static void AttachFamily(IPlayerEntity player, FamilyDto dto, FamilyAuthority authority)
        {
            player.Family = dto;
            player.FamilyCharacter = new CharacterFamilyDto
            {
                Authority = authority,
                CharacterId = player.Character.Id,
                FamilyId = dto.Id,
                Rank = 0
            };
            // change faction ?
        }
    }
}