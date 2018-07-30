﻿using Autofac;

namespace ChickenAPI.Core.IoC
{
    public class Container
    {
        public static ContainerBuilder Builder = new ContainerBuilder();

        public static IContainer Instance { get; private set; }

        public static void Initialize()
        {
            Instance = Builder.Build();
        }
    }
}