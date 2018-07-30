namespace NosSharp.BasicAlgorithm
{
    public interface ILevelBasedDataAlgorithm
    {
        void Initialize();

        long[] Data { get; set; }
    }
}