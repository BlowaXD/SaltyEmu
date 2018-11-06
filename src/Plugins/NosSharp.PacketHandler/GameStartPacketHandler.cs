using System;
using System.Collections.Generic;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.ECS.Entities;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Game.Features.Player;
using ChickenAPI.Game.Features.Skills.Extensions;
using ChickenAPI.Game.Inventory.Args;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets.CharacterSelectionScreen.Client;
using ChickenAPI.Packets.Game.Server.Map;
using ChickenAPI.Packets.Game.Server.Player;

namespace NosSharp.PacketHandler
{
    public class GameStartPacketHandler
    {
        private static readonly IMapManager MapManager = new Lazy<IMapManager>(ChickenContainer.Instance.Resolve<IMapManager>).Value;
        private static readonly IAlgorithmService AlgorithmService = new Lazy<IAlgorithmService>(ChickenContainer.Instance.Resolve<IAlgorithmService>).Value;
        private static readonly ICharacterService CharacterService = new Lazy<ICharacterService>(ChickenContainer.Instance.Resolve<ICharacterService>).Value;
        private static readonly ICharacterSkillService CharacterSkillService = new Lazy<ICharacterSkillService>(ChickenContainer.Instance.Resolve<ICharacterSkillService>).Value;
        private static readonly ICharacterQuickListService CharacterQuicklistService = new Lazy<ICharacterQuickListService>(ChickenContainer.Instance.Resolve<ICharacterQuickListService>).Value;

        public static async void OnGameStart(GameStartPacketBase packetBase, ISession session)
        {
            /*
             * 
             */
            if (session.Player != null)
            {
                return;
            }

            CharacterDto dto = await CharacterService.GetByIdAsync(session.CharacterId);
            IEnumerable<CharacterSkillDto> skills = await CharacterSkillService.GetByCharacterIdAsync(session.CharacterId);
            IEnumerable<CharacterQuicklistDto> quicklist = await CharacterQuicklistService.GetByCharacterIdAsync(session.CharacterId);
            IMapLayer mapLayer = MapManager.GetBaseMapLayer(dto.MapId);
            session.InitializeEntity(new PlayerEntity(session, dto, skills, quicklist));
            session.SendPacket(new TitPacket { ClassType = "Adventurer", Name = dto.Name });
            session.SendPacket(new SayPacket { Message = "┌------------------[SaltyEmu]------------------┐", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket
                { Message = $"XP     : {dto.LevelXp}/{AlgorithmService.GetLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket
                { Message = $"JOBXP  : {dto.JobLevelXp}/{AlgorithmService.GetJobLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket
                { Message = $"HEROXP : {dto.HeroXp}/{AlgorithmService.GetHeroLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket { Message = "└----------------------------------------------┘", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new MapoutPacket());
            session.Player.TransferEntity(mapLayer);
            session.SendPacket(session.Player.GenerateSkiPacket());
            session.SendPacket(session.Player.GenerateFdPacket());
            // rage
            // rank_cool
            // pet basket ? ib 1278 1
            // cleftOfDarkness ? bc 0 0 0
            // sppoint
            // scr 0
            for (byte i = 0; i < 10; i++)
            {
                session.Player.SendPacket(new BnPacket { BnNumber = i, Message = $"SaltyEmu^{i}" });
            }

            // exts
            // MlInfo
            // PClear
            // PInit
            // zzim
            // twk 2
            // act 6
            session.SendPacket(session.Player.GenerateFsPacket());
            // scP
            // scN


            session.Player.EmitEvent(new InventoryInitializeEventArgs());
            session.Player.EmitEvent(new InventoryGeneratePacketDetailsEventArgs());
            session.SendPacket(session.Player.GenerateCondPacket());

            session.SendPacket(session.Player.GenerateGoldPacket());
          //  session.SendPackets(session.Player.GenerateQuicklistPacket());
            // finit
            // Blinit
            // clinit
            // flinit
            // kdlinit

            session.SendPacket(session.Player.GenerateStatCharPacket());
            session.SendPacket(session.Player.GeneratePairyPacket());


            // Session.SendPacket(Session.Character.GenerateGInfo());
            session.SendPacket(session.Player.GenerateGInfoPacket());
            session.Player.Broadcast(session.Player.GenerateGidxPacket());
            //Session.CurrentMapInstance?.Broadcast(Session.Character.GenerateGidx());
            // Session.SendPackets(Session.Character.GetFamilyHistory());
            // Session.SendPacket(Session.Character.GenerateFamilyMember());
            // Session.SendPacket(Session.Character.GenerateFamilyMemberMessage());
            // Session.SendPacket(Session.Character.GenerateFamilyMemberExp());

            // mails
            // quests
        }
    }
}