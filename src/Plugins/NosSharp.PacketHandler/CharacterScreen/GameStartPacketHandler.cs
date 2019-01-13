using System.Collections.Generic;
using System.Threading.Tasks;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Mates;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Game.Helpers;
using ChickenAPI.Game.Inventory.Events;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets.CharacterSelectionScreen.Client;
using ChickenAPI.Packets.Game.Server.Map;
using ChickenAPI.Packets.Game.Server.Player;
using ChickenAPI.Packets.Game.Server.UserInterface;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.CharacterScreen
{
    public class GameStartPacketHandler : GenericSessionPacketHandlerAsync<GameStartPacketBase>
    {
        private readonly IAlgorithmService _algorithmService;
        private readonly ICharacterMateService _characterMateService;
        private readonly ICharacterQuickListService _characterQuicklistService;
        private readonly ICharacterService _characterService;
        private readonly ICharacterSkillService _characterSkillService;
        private readonly IMapManager _mapManager;
        private readonly INpcMonsterService _npcMonsterService;
        private readonly IPlayerManager _playerManager;

        public GameStartPacketHandler(IPlayerManager playerManager, IMapManager mapManager, IAlgorithmService algorithmService, ICharacterService characterService,
            ICharacterSkillService characterSkillService, ICharacterQuickListService characterQuicklistService, ICharacterMateService characterMateService, INpcMonsterService npcMonsterService)
        {
            _playerManager = playerManager;
            _mapManager = mapManager;
            _algorithmService = algorithmService;
            _characterService = characterService;
            _characterSkillService = characterSkillService;
            _characterQuicklistService = characterQuicklistService;
            _characterMateService = characterMateService;
            _npcMonsterService = npcMonsterService;
        }


        protected override async Task Handle(GameStartPacketBase packet, ISession session)
        {
            if (session.Player != null)
            {
                return;
            }

            CharacterDto dto = await _characterService.GetByIdAsync(session.CharacterId);
            IEnumerable<CharacterSkillDto> skills = await _characterSkillService.GetByCharacterIdAsync(session.CharacterId);
            IEnumerable<CharacterQuicklistDto> quicklist = await _characterQuicklistService.GetByCharacterIdAsync(session.CharacterId);
            IEnumerable<CharacterMateDto> mates = await _characterMateService.GetMatesByCharacterIdAsync(session.CharacterId);
            IMapLayer mapLayer = _mapManager.GetBaseMapLayer(dto.MapId);
            session.InitializeEntity(new PlayerEntity(session, dto, skills, quicklist));

            // Register Player
            _playerManager.RegisterPlayer(session.Player);

            await session.Player.SendChatMessage("┌------------------[NosWings]------------------┐", SayColorType.Yellow);
            await session.Player.SendChatMessage($"XP     : {dto.LevelXp}/{_algorithmService.GetLevelXp(dto.Class, dto.Level)}", SayColorType.Yellow);
            await session.Player.SendChatMessage($"JOBXP  : {dto.JobLevelXp}/{_algorithmService.GetJobLevelXp(dto.Class, dto.Level)}", SayColorType.Yellow);
            await session.Player.SendChatMessage($"HEROXP : {dto.HeroXp}/{_algorithmService.GetHeroLevelXp(dto.Class, dto.Level)}", SayColorType.Yellow);
            await session.Player.SendChatMessage("└----------------------------------------------┘", SayColorType.Yellow);
            await session.SendPacketAsync(new TitPacket { ClassType = "Adventurer", Name = dto.Name });
            await session.SendPacketAsync(new MapoutPacket());
            session.Player.Character.SpPoint = 10000;
            session.Player.Character.SpAdditionPoint = 500000;
            session.Player.TransferEntity(mapLayer);
            await session.Player.ActualizeUiSkillList();
            await session.Player.ActualizeUiReputation();
            await session.SendPacketAsync(new RagePacket { RagePoints = 0, RagePointsMax = 250000 });
            // rank_cool
            // pet basket ? ib 1278 1
            // cleftOfDarkness ? bc 0 0 0
            await session.Player.ActualiseUiSpPoints();
            // scr 0
            for (byte i = 0; i < 10; i++)
            {
                session.Player.SendPacket(new BnPacket { BnNumber = i, Message = $"SaltyEmu^{i}" });
            }

            // exts
            // MlInfo
            await session.SendPacketAsync(new PClearPacket());
            // PInit
            await session.SendPacketAsync(new ZzimPacket());
            // twk 2
            await session.SendPacketAsync(new Act6Packet());
            await session.SendPacketAsync(session.Player.GenerateFsPacket());
            // scP
            // scN

            session.Player.EmitEvent(new InventoryLoadEvent());
            session.Player.EmitEvent(new InventoryRequestDetailsEvent());
            await session.Player.ActualizePlayerCondition();
            await session.Player.ActualizeUiGold();
            await session.Player.ActualizeUiQuicklist();
            await session.Player.ActualizeUiFriendList();
            await session.Player.ActualizeUiBlackList();
            // Blinit
            // clinit
            // flinit
            // kdlinit

            await session.SendPacketAsync(session.Player.GenerateStatCharPacket());
            await session.SendPacketAsync(session.Player.GeneratePairyPacket());

            await session.SendPacketAsync(session.Player.GenerateGInfoPacket());
            session.Player.Broadcast(session.Player.GenerateGidxPacket());
            // Session.SendPackets(Session.Character.GetFamilyHistory());
            // await session.SendPacketAsync(Session.Character.GenerateFamilyMember());
            // await session.SendPacketAsync(Session.Character.GenerateFamilyMemberMessage());
            // await session.SendPacketAsync(Session.Character.GenerateFamilyMemberExp());

            // mails
            // quests

            foreach (CharacterMateDto s in mates)
            {
                var mate = new MateEntity(session.Player, s, _npcMonsterService.GetById(s.NpcMonsterId));
                session.Player.Mates.Add(mate);
            }
        }
    }
}