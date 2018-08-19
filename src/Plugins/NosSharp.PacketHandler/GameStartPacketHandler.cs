using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.TransferObjects.Character;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Features.Inventory;
using ChickenAPI.Game.Features.Inventory.Args;
using ChickenAPI.Game.Features.Inventory.Extensions;
using ChickenAPI.Game.Features.Movement.Extensions;
using ChickenAPI.Game.Features.Player.Extensions;
using ChickenAPI.Game.Features.Skills.Extensions;
using ChickenAPI.Game.Game.Components;
using ChickenAPI.Game.Managers;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Packets.CharacterScreen.Client;
using ChickenAPI.Game.Packets.Game.Server;
using ChickenAPI.Game.Packets.Game.Server.Map;

namespace NosSharp.PacketHandler
{
    public class GameStartPacketHandler
    {
        public static async void OnGameStart(GameStartPacketBase packetBase, ISession session)
        {
            /*
             * 
             */
            if (session.Player != null)
            {
                return;
            }

            var mapManager = Container.Instance.Resolve<IMapManager>();
            CharacterDto dto = await Container.Instance.Resolve<ICharacterService>().GetByIdAsync(session.CharacterId);
            var algorithm = Container.Instance.Resolve<IAlgorithmService>();
            IMapLayer mapLayer = mapManager.GetBaseMapLayer(dto.MapId);
            session.InitializeEntity(new PlayerEntity(session, dto));
            session.SendPacket(new TitPacket { ClassType = "Adventurer", Name = session.Player.GetComponent<NameComponent>().Name});
            session.SendPacket(new SayPacket { Message = "┌------------------[SaltyEmu]------------------┐", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket { Message = $"XP     : {dto.LevelXp}/{algorithm.GetLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket { Message = $"JOBXP  : {dto.JobLevelXp}/{algorithm.GetJobLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket { Message = $"HEROXP : {dto.HeroXp}/{algorithm.GetHeroLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
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
                session.Player.SendPacket(new BnPacket { BnNumber = i, Message = $"SaltyEmu^{i}"});
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


            session.Player.NotifyEventHandler<InventoryEventHandler>(new InventoryInitializeEventArgs());
            session.Player.NotifyEventHandler<InventoryEventHandler>(new InventoryGeneratePacketDetailsEventArgs());
            session.SendPacket(session.Player.GenerateCondPacket());

            session.SendPacket(session.Player.GenerateGoldPacket());
            session.SendPackets(session.Player.GenerateQuicklistPacket());
            // finit
            // Blinit
            // clinit
            // flinit
            // kdlinit

            session.SendPacket(session.Player.GenerateStatCharPacket());
            session.SendPacket(session.Player.GeneratePairyPacket());



            // Session.SendPacket(Session.Character.GenerateGInfo());
            // Session.CurrentMapInstance?.Broadcast(Session.Character.GenerateGidx());
            // Session.SendPackets(Session.Character.GetFamilyHistory());
            // Session.SendPacket(Session.Character.GenerateFamilyMember());
            // Session.SendPacket(Session.Character.GenerateFamilyMemberMessage());
            // Session.SendPacket(Session.Character.GenerateFamilyMemberExp());

            // mails
            // quests

        }
    }
}