namespace ChickenAPI.Core.Maths
{
    public interface IRandomGenerator
    {
        int Next(int min, int max);

        int Next(int max);

        double Next();
    }
}