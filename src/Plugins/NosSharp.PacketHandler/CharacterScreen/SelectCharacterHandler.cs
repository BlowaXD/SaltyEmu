using System;
using System.Threading.Tasks;
using ChickenAPI.Data.Character;
using ChickenAPI.Game.Network;
using ChickenAPI.Packets.CharacterSelectionScreen.Client;
using ChickenAPI.Packets.CharacterSelectionScreen.Server;
using NW.Plugins.PacketHandling.Utils;

namespace NW.Plugins.PacketHandling.CharacterScreen
{
    public class SelectCharacterHandler : GenericSessionPacketHandlerAsync<SelectPacketBase>
    {
        private readonly ICharacterService _characterService;

        public SelectCharacterHandler(ICharacterService characterService) => _characterService = characterService;

        protected override async Task Handle(SelectPacketBase packet, ISession session)
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
                await session.SendPacketAsync(new OkPacketBase());
            }
            catch (Exception ex)
            {
                Log.Error("Select characterEntity failed.", ex);
            }
        }
    }
}