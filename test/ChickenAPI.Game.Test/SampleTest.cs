using Autofac;
using ChickenAPI.Core.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltyEmu.DatabasePlugin.Context;

namespace ChickenAPI.Game.Test
{
    [TestClass]
    public class SampleTest
    {
        [TestInitialize]
        public void Initialize()
        {
            InjectDependencies();
            ChickenContainer.Initialize();
            LoadPlayer();
            InitializeTestComponents();
            // initialize TestComponents
        }

        private void InjectDependencies()
        {
            ChickenContainer.Builder.Register(s =>
            {
                DbContextOptionsBuilder<SaltyDbContext> options = new DbContextOptionsBuilder<SaltyDbContext>().UseInMemoryDatabase("test");
                return new SaltyDbContext(options.Options);
            }).As<SaltyDbContext>().InstancePerDependency();
        }

        private void LoadPlayer()
        {
        }

        private void InitializeTestComponents()
        {
        }

        [TestMethod]
        public void CreatePlayerEntityTest()
        {
        }
    }
}