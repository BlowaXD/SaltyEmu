using System;
using System.Threading.Tasks;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Character;
using ChickenAPI.Game._Network;
using ChickenAPI.Packets.ClientPackets.CharacterSelectionScreen;
using ChickenAPI.Packets.ServerPackets.CharacterSelectionScreen;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.CharacterScreen
{
    public class SelectCharacterHandler : GenericSessionPacketHandlerAsync<SelectPacket>
    {
        private readonly ICharacterService _characterService;

        public SelectCharacterHandler(ILogger log, ICharacterService characterService) : base(log) => _characterService = characterService;

        protected override async Task Handle(SelectPacket packet, ISession session)
        {
            try
            {
                if (session?.Account == null)
                {
                    return;
                }

                CharacterDto characterDto = await _characterService.GetByAccountIdAndSlotAsync(session.Account.Id, packet.Slot);
                if (characterDto == null)
                {
                    return;
                }

                session.InitializeCharacterId(characterDto.Id);
                await session.SendPacketAsync(new OkPacket());
            }
            catch (Exception ex)
            {
                Log.Error("Select characterEntity failed.", ex);
            }
        }
    }
}