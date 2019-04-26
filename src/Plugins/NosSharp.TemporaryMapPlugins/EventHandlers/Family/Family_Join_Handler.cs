using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Families;
using ChickenAPI.Enums.Game.Families;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Packets.Enumerations;

namespace SaltyEmu.BasicPlugin.EventHandlers.Family
{
    public class Family_Join_Handler : GenericEventPostProcessorBase<FamilyJoinEvent>
    {
        private readonly ICharacterFamilyService _characterFamilyService;

        public Family_Join_Handler(ILogger log, ICharacterFamilyService characterFamilyService) : base(log) => _characterFamilyService = characterFamilyService;

        protected override async Task Handle(FamilyJoinEvent e, CancellationToken cancellation)
        {
            if (e.Player.HasFamily)
            {
                Log.Info("[FAMILY][JOIN] ALREADY_IN_FAMILY");
                return;
            }

            if (e.ExpectedAuthority == FamilyAuthority.Head)
            {
                Log.Info("[FAMILY][JOIN] CANT_HAVE_TWO_LEADERS");
                return;
            }

            await AttachFamilyAsync(e.Player, e.Family, e.ExpectedAuthority);
            await e.Player.BroadcastAsync(e.Player.GenerateGidxPacket());
            await e.Player.SendPacketAsync(e.Player.GenerateGInfoPacket());
        }

        private async Task AttachFamilyAsync(IPlayerEntity player, FamilyDto family, FamilyAuthority authority)
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
                Rank = FamilyMemberRank.Nothing
            });
        }
    }
}