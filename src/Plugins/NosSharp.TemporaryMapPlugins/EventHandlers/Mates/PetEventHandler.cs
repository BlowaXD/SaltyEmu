using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using ChickenAPI.Core.Events;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Events;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Player.Extension;

namespace SaltyEmu.BasicPlugin.EventHandlers
{
    public class PetEventHandler : GenericEventPostProcessorBase<AddPetEvent>
    {
        private readonly IGameEntityFactory _entityFactory;
        private readonly INpcMonsterService _npcMonsterService;

        public PetEventHandler(INpcMonsterService npcMonsterService, IGameEntityFactory entityFactory)
        {
            _npcMonsterService = npcMonsterService;
            _entityFactory = entityFactory;
        }

        public static void AddPet(IPlayerEntity player, AddPetEvent e)
        {
        }

        protected override async Task Handle(AddPetEvent e, CancellationToken cancellation)
        {
            if (!(e.Sender is IPlayerEntity player))
            {
                return;
            }

            NpcMonsterDto heldMonster = _npcMonsterService.GetById(e.MonsterVnum);

            if (heldMonster == null)
            {
                return;
            }

            var mate = new CharacterMateDto
            {
                NpcMonsterId = (short)e.MonsterVnum,
                CanPickUp = false,
                CharacterId = player.Id,
                Attack = 0,
                Defence = 0,
                Direction = 0,
                Experience = 0,
                Hp = heldMonster.MaxHp,
                Level = 1,
                IsSummonable = false,
                IsTeamMember = true,
                Loyalty = 1000,
                MapX = player.Position.X,
                MapY = player.Position.Y,
                Mp = heldMonster.MaxMp,
                Name = heldMonster.Name,
                Skin = 0,
                Agility = 0,
                MateType = e.MateType
            };

            IMateEntity mateEn = await _entityFactory.CreateNewMateEntityAsync(player, mate);
            player.AddPet(mateEn);
            await player.SendPacketAsync(player.GenerateInfoBubble("PET_LEAVE_BEAD"));
        }
    }
}