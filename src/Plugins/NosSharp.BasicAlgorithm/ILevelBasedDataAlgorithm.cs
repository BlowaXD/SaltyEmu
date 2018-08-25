namespace NosSharp.BasicAlgorithm
{
    public interface ILevelBasedDataAlgorithm
    {
        long[] Data { get; set; }
        void Initialize();
    }
}