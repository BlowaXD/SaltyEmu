using ChickenAPI.Core.Events;

namespace ChickenAPI.Game.Features.Quicklist.Args
{
    public class GenerateQuickListArgs : ChickenEventArgs
    {

        public bool IsSkill { get; set; }
        public short Q1 { get; set; }
        public short Q2 { get; set; }

        public short Data1 { get; set; }
        public short Data2 { get; set; }
    }
}
