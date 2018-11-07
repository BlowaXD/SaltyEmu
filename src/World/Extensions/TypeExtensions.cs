using System;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace World.Extensions
{
    /// <summary>
    /// https://stackoverflow.com/questions/6582259/fast-creation-of-objects-instead-of-activator-createinstancetype
    /// https://github.com/KSemenenko/CreateInstance
    /// https://blogs.msdn.microsoft.com/seteplia/2017/02/01/dissecting-the-new-constraint-in-c-a-perfect-example-of-a-leaky-abstraction/
    /// Thanks to these articles, it was designed to fasten the PacketFactory's packet instanciation
    /// </summary>
    public static class TypeExtension
    {
        private static readonly ConcurrentDictionary<Type, Func<object>> Constructors = new ConcurrentDictionary<Type, Func<object>>();

        /// <summary>
        /// Gets the default constructor delegate (empty parameters)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Func<object> GetDefaultConstructorDelegate(this Type type) => (Func<object>)GetConstructorDelegate(type, typeof(Func<object>));

        /// <summary>
        /// Retrieves the constructor of the given type based on the given delegate type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="delegateType"></param>
        /// <returns></returns>
        public static Delegate GetConstructorDelegate(this Type type, Type delegateType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (delegateType == null)
            {
                throw new ArgumentNullException(nameof(delegateType));
            }

            Type[] genericArguments = delegateType.GetGenericArguments();
            Type[] argTypes = genericArguments.Length > 1 ? genericArguments.Take(genericArguments.Length - 1).ToArray() : Type.EmptyTypes;

            ConstructorInfo constructor = type.GetConstructor(argTypes);
            if (constructor == null)
            {
                if (argTypes.Length == 0)
                {
                    throw new InvalidOperationException($"Type '{type.Name}' doesn't have a parameterless constructor.");
                }

                throw new InvalidOperationException($"Type '{type.Name}' doesn't have the requested constructor.");
            }

            var dynamicMethod = new DynamicMethod("DM$OBJ_FACTORY_" + type.Name, type, argTypes, type);
            ILGenerator ilGen = dynamicMethod.GetILGenerator();
            for (int i = 0; i < argTypes.Length; i++)
            {
                ilGen.Emit(OpCodes.Ldarg, i);
            }

            ilGen.Emit(OpCodes.Newobj, constructor);
            ilGen.Emit(OpCodes.Ret);
            return dynamicMethod.CreateDelegate(delegateType);
        }

        /// <summary>
        /// This extension is much faster than 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type type)
        {
            if (Constructors.TryGetValue(type, out Func<object> constructor))
            {
                return constructor();
            }

            constructor = type.GetDefaultConstructorDelegate();
            Constructors.TryAdd(type, constructor);
            return constructor();
        }
    }
}