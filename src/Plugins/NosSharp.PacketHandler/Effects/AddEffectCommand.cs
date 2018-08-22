using ChickenAPI.Enums;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Features.Effects;
using ChickenAPI.Game.Features.Effects.Args;
using ChickenAPI.Game.Packets;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Attributes;

namespace NosSharp.PacketHandler.Effects
{
    public class AddEffectCommandHandler
    {
        public static void OnAddEffectCommand(AddEffectCommand packet, IPlayerEntity player)
        {
            player.NotifyEventHandler<EffectEventHandler>(new AddEffectArgument
            {
                EffectId = packet.EffectId,
                Cooldown = packet.Cooldown
            });
        }
    }

    [PacketHeader("$AddEffect", Authority = AuthorityType.GameMaster)]
    public class AddEffectCommand : PacketBase
    {
        [PacketIndex(0)]
        public long EffectId { get; set; }

        [PacketIndex(1)]
        public long Cooldown { get; set; }
    }
}