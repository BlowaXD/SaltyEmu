using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NosSharp.PacketHandler.Utils
{
    public static class AssemblyExtension
    {
        public static object CreateInstance<T>(this T tmp)
        {
            return Activator.CreateInstance(typeof(T));
        }
        public static IEnumerable<T> GetInstancesOfImplementingTypes<T>(this Assembly assembly)
        {
            return from t in assembly.GetTypes() where typeof(T).IsAssignableFrom(t) select (T)t.CreateInstance();
        }
    }
}