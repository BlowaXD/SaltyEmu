using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace World.Extensions
{
    public static class AssemblyExtension
    {
        public static IEnumerable<T> GetInstancesOfImplementingTypes<T>(this Assembly assembly) => from t in assembly.GetTypes() where typeof(T).IsAssignableFrom(t) select (T)t.CreateInstance();
    }
}