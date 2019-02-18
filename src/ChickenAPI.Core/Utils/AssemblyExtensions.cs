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

        public static List<Type> GetTypesImplementingInterface(this Assembly assembly, params Type[] types)
        {
            List<Type> list = new List<Type>();
            foreach (Type type in types)
            {
                list.AddRange(assembly.GetTypesImplementingInterface(type));
            }

            return list;
        }

        public static List<Type> GetTypesImplementingInterface<T>(this Assembly assembly)
        {
            return assembly.GetTypesImplementingInterface(typeof(T));
        }

        public static List<Type> GetTypesImplementingInterface(this Assembly assembly, Type type)
        {
            List<Type> list = new List<Type>();
            list.AddRange(assembly.GetTypes().Where(s => s.ImplementsInterface(type)));

            return list;
        }

        public static bool ImplementsInterface<T>(this Type type)
        {
            return type.GetInterfaces().Any(s => s == typeof(T));
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(s => s == interfaceType);
        }

        public static List<Type> GetTypesImplementingGenericClass(this Assembly assembly, params Type[] types)
        {
            List<Type> list = new List<Type>();
            foreach (Type type in types)
            {
                list.AddRange(assembly.GetTypesImplementingGenericClass(type));
            }

            return list;
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