using Autofac;
using AutoMapper;
using ChickenAPI.Core.IoC;
using ChickenAPI.Game.Data.AccessLayer.Account;
using ChickenAPI.Game.Data.AccessLayer.BCard;
using ChickenAPI.Game.Data.AccessLayer.Character;
using ChickenAPI.Game.Data.AccessLayer.Drop;
using ChickenAPI.Game.Data.AccessLayer.Item;
using ChickenAPI.Game.Data.AccessLayer.Map;
using ChickenAPI.Game.Data.AccessLayer.NpcMonster;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Game.Data.AccessLayer.Skill;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Services.Account;
using SaltyEmu.DatabasePlugin.Services.BCard;
using SaltyEmu.DatabasePlugin.Services.Card;
using SaltyEmu.DatabasePlugin.Services.Character;
using SaltyEmu.DatabasePlugin.Services.Drop;
using SaltyEmu.DatabasePlugin.Services.Item;
using SaltyEmu.DatabasePlugin.Services.Map;
using SaltyEmu.DatabasePlugin.Services.NpcMonster;
using SaltyEmu.DatabasePlugin.Services.Shop;
using SaltyEmu.DatabasePlugin.Services.Skill;
using SaltyEmu.DatabasePlugin.Utils;

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
            IMapper mapper = NosSharpDatabasePluginMapper.ConfigureMapper().CreateMapper();
            ChickenContainer.Builder.Register(s => new SkillDao(s.Resolve<SaltyDbContext>(), mapper)).As<ISkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new BCardDao(s.Resolve<SaltyDbContext>(), mapper)).As<IBCardService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CardDao(s.Resolve<SaltyDbContext>(), mapper)).As<ICardService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ItemDao(s.Resolve<SaltyDbContext>(), mapper)).As<IItemService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new NpcMonsterDao(s.Resolve<SaltyDbContext>(), mapper)).As<INpcMonsterService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new NpcMonsterSkillDao(s.Resolve<SaltyDbContext>(), mapper)).As<INpcMonsterSkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapDao(s.Resolve<SaltyDbContext>(), mapper)).As<IMapService>().InstancePerLifetimeScope();

            ChickenContainer.Builder.Register(s => new AccountDao(s.Resolve<SaltyDbContext>(), mapper)).As<IAccountService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterDao(s.Resolve<SaltyDbContext>(), mapper, null)).As<ICharacterService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterMateDao(s.Resolve<SaltyDbContext>(), mapper)).As<ICharacterMateService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterItemDao(s.Resolve<SaltyDbContext>(), mapper)).As<IItemInstanceService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterSkillDao(s.Resolve<SaltyDbContext>(), mapper)).As<ICharacterSkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterQuickListDao(s.Resolve<SaltyDbContext>(), mapper)).As<ICharacterQuickListService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterFamilyDao(s.Resolve<SaltyDbContext>(), mapper)).As<ICharacterFamilyService>().InstancePerLifetimeScope();

            ChickenContainer.Builder.Register(s => new MapMonsterDao(s.Resolve<SaltyDbContext>(), mapper)).As<IMapMonsterService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapNpcDao(s.Resolve<SaltyDbContext>(), mapper)).As<IMapNpcService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapPortalDao(s.Resolve<SaltyDbContext>(), mapper)).As<IPortalService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ShopDao(s.Resolve<SaltyDbContext>(), mapper)).As<IShopService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ShopItemDao(s.Resolve<SaltyDbContext>(), mapper)).As<IShopItemService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ShopSkillDao(s.Resolve<SaltyDbContext>(), mapper)).As<IShopSkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new RecipeDao(s.Resolve<SaltyDbContext>(), mapper)).As<IRecipeService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new RecipeItemDao(s.Resolve<SaltyDbContext>(), mapper)).As<IRecipeItemService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new DropDao(s.Resolve<SaltyDbContext>(), mapper)).As<IDropService>().InstancePerLifetimeScope();
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