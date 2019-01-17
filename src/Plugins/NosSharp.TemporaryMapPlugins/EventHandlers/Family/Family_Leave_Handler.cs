using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Families.Extensions;

namespace SaltyEmu.BasicPlugin.EventHandlers.Family
{
    public class Family_Leave_Handler : GenericEventPostProcessorBase<FamilyLeaveEvent>
    {
        private readonly ICharacterFamilyService _characterFamilyService;

        public Family_Leave_Handler(ICharacterFamilyService characterFamilyService)
        {
            _characterFamilyService = characterFamilyService;
        }

        protected override async Task Handle(FamilyLeaveEvent e, CancellationToken cancellation)
        {
            IPlayerEntity player = e.Player;
            if (player.IsFamilyLeader)
            {
                Log.Warn("CANT_LEAVE_FAMILY_LEADER");
                return;
            }

            await DetachFamily(player);
            await player.BroadcastAsync(player.GenerateGidxPacket());
        }

        public async Task DetachFamily(IPlayerEntity player)
        {
            if (player.FamilyCharacter == null)
            {
                return;
            }

            await _characterFamilyService.DeleteByIdAsync(player.FamilyCharacter.Id);
            player.FamilyCharacter = null;
            player.Family = null;
            // globally update family
        }
    }
}