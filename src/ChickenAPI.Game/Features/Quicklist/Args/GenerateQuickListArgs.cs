using ChickenAPI.Game.Events;

namespace ChickenAPI.Game.Features.Quicklist.Args
{
    public class GenerateQuickListArgs : ChickenEventArgs
    {

        public short Type { get; set; } // New enum ? 0 = Formation , = 1 Skill , 2 = SwitchPlace , 3 = remove
        public short Q1 { get; set; }
        public short Q2 { get; set; }

        public short Data1 { get; set; }
        public short Data2 { get; set; }
    }
}
