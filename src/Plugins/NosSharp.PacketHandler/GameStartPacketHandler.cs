using Autofac;
using ChickenAPI.Data.AccessLayer;
using ChickenAPI.Data.AccessLayer.Character;
using ChickenAPI.Data.AccessLayer.Server;
using ChickenAPI.Data.TransferObjects;
using ChickenAPI.Data.TransferObjects.Character;
using ChickenAPI.Enums.Game.Entity;
using ChickenAPI.Enums.Packets;
using ChickenAPI.Game.Components;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Maps;
using ChickenAPI.Game.Network;
using ChickenAPI.Game.Systems.Inventory;
using ChickenAPI.Game.Systems.Inventory.Args;
using ChickenAPI.Managers;
using ChickenAPI.Packets.CharacterScreen.Client;
using ChickenAPI.Packets.Game.Server;
using ChickenAPI.Packets.Game.Server.Map;
using ChickenAPI.Utils;

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
            session.SendPacket(new SayPacket { Message = "┌------------------[NosWings]------------------┐", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket { Message = $"XP     : {dto.LevelXp}/{algorithm.GetLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket { Message = $"JOBXP  : {dto.JobLevelXp}/{algorithm.GetJobLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket { Message = $"HEROXP : {dto.HeroXp}/{algorithm.GetHeroLevelXp(dto.Class, dto.Level)}", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new SayPacket { Message = "└----------------------------------------------┘", Type = SayColorType.Yellow, VisualType = VisualType.Character });
            session.SendPacket(new MapoutPacket());
            session.Player.TransferEntity(mapLayer);
            session.SendPacket(new FdPacket { Dignity = (int)dto.Dignity, DignityIcon = 1, Reput = dto.Reput, ReputIcon = 1 });
            // scr 0
            for (byte i = 0; i < 10; i++)
            {
                session.Player.SendPacket(new BnPacket { BnNumber = i, Message = $"NosWings^{i}"});
            }

            session.Player.EntityManager.NotifySystem<InventorySystem>(session.Player, new InventoryInitializeEventArgs());
            session.Player.EntityManager.NotifySystem<InventorySystem>(session.Player, new InventoryGeneratePacketDetailsEventArgs());
            session.SendPacket(new CondPacketBase(session.Player));
        }
    }
}