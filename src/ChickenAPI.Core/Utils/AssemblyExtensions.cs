using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChickenAPI.Core.Utils
{
    public static class AssemblyExtensions
    {
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            if (generic == toCheck) return false;
            while (toCheck != null && toCheck != typeof(object))
            {
                Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static List<Type> GetTypesImplementingGenericClass(this Assembly assembly, Type type)
        {
            return assembly.GetTypes().Where(s => IsSubclassOfRawGeneric(type, s)).ToList();
        }

        public static List<Type> GetTypesDerivedFrom<T>(this Assembly assembly)
        {
            return assembly.GetTypes().Where(s => s.IsSubclassOf(typeof(T))).ToList();
        }
    }
}