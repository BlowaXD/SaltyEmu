namespace SaltyEmu.BasicAlgorithmPlugin
{
    public interface ILevelBasedDataAlgorithm
    {
        long[] Data { get; set; }
        void Initialize();
    }
}