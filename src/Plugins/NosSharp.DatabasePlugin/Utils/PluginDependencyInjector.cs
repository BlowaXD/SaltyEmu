using System;
using Autofac;
using AutoMapper;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Drop;
using ChickenAPI.Data.Enums.Game.Character;
using ChickenAPI.Data.Families;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Shop;
using ChickenAPI.Data.Skills;
using ChickenAPI.Packets.Enumerations;
using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Context;
using SaltyEmu.DatabasePlugin.Services.Account;
using SaltyEmu.DatabasePlugin.Services.BCard;
using SaltyEmu.DatabasePlugin.Services.Card;
using SaltyEmu.DatabasePlugin.Services.Character;
using SaltyEmu.DatabasePlugin.Services.Drop;
using SaltyEmu.DatabasePlugin.Services.Families;
using SaltyEmu.DatabasePlugin.Services.Item;
using SaltyEmu.DatabasePlugin.Services.Map;
using SaltyEmu.DatabasePlugin.Services.NpcMonster;
using SaltyEmu.DatabasePlugin.Services.Shop;
using SaltyEmu.DatabasePlugin.Services.Skill;

namespace SaltyEmu.DatabasePlugin.Utils
{
    public class PluginDependencyInjector
    {
        public static void RegisterMapping()
        {
            ChickenContainer.Builder.Register(s => NosSharpDatabasePluginMapper.ConfigureMapper().CreateMapper()).As<IMapper>().InstancePerDependency();
        }

        public static void RegisterDbContext(Configuration.DatabaseConfiguration configuration)
        {
            ChickenContainer.Builder.Register(s =>
            {
                DbContextOptionsBuilder<SaltyDbContext> options = new DbContextOptionsBuilder<SaltyDbContext>().UseSqlServer(configuration.ToString());

                return new SaltyDbContext(options.Options);
            }).As<SaltyDbContext>().InstancePerDependency();
        }

        public static void RegisterDaos()
        {
            var characterConf = new CharacterDto
            {
                Class = CharacterClassType.Adventurer,
                Gender = GenderType.Male,
                HairColor = HairColorType.Black,
                HairStyle = HairStyleType.HairStyleA,
                Hp = 221,
                JobLevel = 20,
                Level = 15,
                MapId = 1,
                MapX = 78,
                MapY = 109,
                Mp = 221,
                MaxMateCount = 10,
                Gold = 15000,
                SpPoint = 10000,
                SpAdditionPoint = 0,
                Name = "template",
                Slot = 0,
                AccountId = 0,
                MinilandMessage = "Welcome",
                State = CharacterState.Active
            };
            // data
            //ChickenContainer.Builder.RegisterTypes(typeof(PluginDependencyInjector).Assembly.GetTypes()).AsImplementedInterfaces().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IMapService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new SkillDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<ISkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new BCardDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IBCardService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CardDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<ICardService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ItemDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IItemService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new NpcMonsterDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<INpcMonsterService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new NpcMonsterSkillDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<INpcMonsterSkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new DropDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IDropService>().InstancePerLifetimeScope();

            // character
            Console.WriteLine("Registering AccountDAO");
            ChickenContainer.Builder.Register(s => new AccountDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IAccountService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), characterConf, s.Resolve<ILogger>())).As<ICharacterService>()
                .InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterMateDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<ICharacterMateService>()
                .InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterItemDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IItemInstanceService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterSkillDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<ICharacterSkillService>()
                .InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterQuickListDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<ICharacterQuickListService>()
                .InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterFamilyDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<ICharacterFamilyService>()
                .InstancePerLifetimeScope();

            // family
            ChickenContainer.Builder.Register(s => new FamilyDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IFamilyService>().InstancePerLifetimeScope();

            // map elements
            ChickenContainer.Builder.Register(s => new MapMonsterDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IMapMonsterService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapNpcDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IMapNpcService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapPortalDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IPortalService>().InstancePerLifetimeScope();

            // shops
            ChickenContainer.Builder.Register(s => new ShopDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IShopService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ShopItemDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IShopItemService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ShopSkillDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IShopSkillService>().InstancePerLifetimeScope();

            // recipes
            ChickenContainer.Builder.Register(s => new RecipeDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IRecipeService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new RecipeItemDao(s.Resolve<SaltyDbContext>(), s.Resolve<IMapper>(), s.Resolve<ILogger>())).As<IRecipeItemService>().InstancePerLifetimeScope();
        }

        public static void RegisterDependencies()
        {
            RegisterMapping();
            RegisterDaos();
        }
    }
}