using ChickenAPI.Enums;
using ChickenAPI.Game.Effects.Args;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Packets;
using ChickenAPI.Packets.Attributes;

namespace NosSharp.PacketHandler.Effects
{
    public class AddEffectCommandHandler
    {
        public static void OnAddEffectCommand(AddEffectCommand packet, IPlayerEntity player)
        {
            player.EmitEvent(new AddEffectArgument
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