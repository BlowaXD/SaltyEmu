namespace NosSharp.Pathfinder.Utils
{
    public static class Heuristics
    {
        /// <summary>
        /// returns the addition of the x and y distances
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static int Manhattan(int dx, int dy) => dx + dy;
    }
}
