using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Family
{
    public class Family_Creation_Handler : GenericEventPostProcessorBase<FamilyCreationEvent>
    {
        private readonly ICharacterFamilyService _characterFamilyService;
        private readonly IFamilyService _familyService;


        public Family_Creation_Handler(ILogger log, ICharacterFamilyService characterFamilyService, IFamilyService familyService) : base(log)
        {
            _characterFamilyService = characterFamilyService;
            _familyService = familyService;
        }

        protected override async Task Handle(FamilyCreationEvent e, CancellationToken cancellation)
        {
            if (_familyService.GetByName(e.FamilyName) != null)
            {
                Log.Info("[FAMILY][CREATION] NAME_ALREADY_TAKEN");
                return;
            }

            if (e.Leader.HasFamily)
            {
                Log.Info("[FAMILY][CREATION] ALREADY_IN_FAMILY");
                return;
            }

            var family = new FamilyDto
            {
                Name = e.FamilyName,
                FamilyFaction = FactionType.Angel,
                FamilyHeadGender = e.Leader.Character.Gender,
                FamilyMessage = string.Empty,
                MaxSize = 50,
            };
            family = await _familyService.SaveAsync(family);
            // todo family object shared across all entities
            await AttachFamily(e.Leader, family, FamilyAuthority.Head);
            await e.Leader.BroadcastAsync(e.Leader.GenerateGidxPacket());
            await e.Leader.SendPacketAsync(e.Leader.GenerateGInfoPacket());

            if (e.Assistants == null)
            {
                return;
            }

            foreach (IPlayerEntity player in e.Assistants)
            {
                if (player.HasFamily)
                {
                    continue;
                }

                await AttachFamily(player, family, FamilyAuthority.Assistant);
                await player.BroadcastAsync(player.GenerateGidxPacket());
                await player.SendPacketAsync(player.GenerateGInfoPacket());
            }
        }

        private async Task AttachFamily(IPlayerEntity player, FamilyDto family, FamilyAuthority authority)
        {
            if (family == null)
            {
                return;
            }

            player.Family = family;
            player.FamilyCharacter = await _characterFamilyService.SaveAsync(new CharacterFamilyDto
            {
                Authority = authority,
                CharacterId = player.Character.Id,
                FamilyId = family.Id,
                Rank = 0
            });
        }
    }
}