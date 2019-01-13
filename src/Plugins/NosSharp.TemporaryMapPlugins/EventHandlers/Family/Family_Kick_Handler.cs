using System.Threading;
using System.Threading.Tasks;
using ChickenAPI.Core.Events;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Families.Events;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Game.Managers;

namespace SaltyEmu.BasicPlugin.EventHandlers.Family
{
    public class Family_Kick_Handler : GenericEventPostProcessorBase<FamilyKickEvent>
    {
        private readonly IPlayerManager _playerManager;
        private readonly ICharacterService _characterService;
        private readonly ICharacterFamilyService _characterFamilyService;

        public Family_Kick_Handler(IPlayerManager playerManager, ICharacterFamilyService characterFamilyService)
        {
            _playerManager = playerManager;
            _characterFamilyService = characterFamilyService;
        }

        protected override async Task Handle(FamilyKickEvent e, CancellationToken cancellation)
        {
            IPlayerEntity player = _playerManager.GetPlayerByCharacterName(e.CharacterName);
            if (player == null)
            {
                await FamilyKickOffline(e);
                return;
            }

            await DetachFamily(player);
            player.Broadcast(player.GenerateGidxPacket());
        }

        public async Task FamilyKickOffline(FamilyKickEvent e)
        {
            var tmp = await _characterService.GetActiveByNameAsync(e.CharacterName);
            await _characterFamilyService.DeleteByIdAsync(tmp.Id);
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