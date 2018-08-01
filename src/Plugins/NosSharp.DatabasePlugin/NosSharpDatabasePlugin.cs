using System;
using Autofac;
using AutoMapper;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.AccessLayer.Account;
using ChickenAPI.Data.AccessLayer.BCard;
using ChickenAPI.Data.AccessLayer.Character;
using ChickenAPI.Data.AccessLayer.Item;
using ChickenAPI.Data.AccessLayer.Map;
using ChickenAPI.Data.AccessLayer.NpcMonster;
using ChickenAPI.Data.AccessLayer.Shop;
using ChickenAPI.Data.AccessLayer.Skill;
using ChickenAPI.Data.TransferObjects.Character;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Data.AccessLayer.Shop;
using ChickenAPI.Plugins;
using Microsoft.EntityFrameworkCore;
using NosSharp.DatabasePlugin.Configuration;
using NosSharp.DatabasePlugin.Context;
using NosSharp.DatabasePlugin.Services.Account;
using NosSharp.DatabasePlugin.Services.BCard;
using NosSharp.DatabasePlugin.Services.Card;
using NosSharp.DatabasePlugin.Services.Character;
using NosSharp.DatabasePlugin.Services.Item;
using NosSharp.DatabasePlugin.Services.Map;
using NosSharp.DatabasePlugin.Services.NpcMonster;
using NosSharp.DatabasePlugin.Services.Shop;
using NosSharp.DatabasePlugin.Services.Skill;
using NosSharp.DatabasePlugin.Utils;

namespace NosSharp.DatabasePlugin
{
    public class NosSharpDatabasePlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<NosSharpDatabasePlugin>();
        private readonly string _configurationFilePath = $"plugins/config/{nameof(NosSharpDatabasePlugin)}/conf.json";
        private readonly string _characterConfPath = $"plugins/config/{nameof(NosSharpDatabasePlugin)}/character_on_creation.json";
        private DatabaseConfiguration _configuration;
        private static IMapper _mapper;
        private static CharacterDto _characterConf;
        public string Name => nameof(NosSharpDatabasePlugin);

        private static void RegisterMapping()
        {
            _mapper = NosSharpDatabasePluginMapper.ConfigureMapper().CreateMapper();
        }

        private static void RegisterDependencies()
        {
            // data
            Container.Builder.Register(s => new SkillDao(s.Resolve<NosSharpContext>(), _mapper)).As<ISkillService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new BCardDao(s.Resolve<NosSharpContext>(), _mapper)).As<IBCardService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new CardDao(s.Resolve<NosSharpContext>(), _mapper)).As<ICardService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new ItemDao(s.Resolve<NosSharpContext>(), _mapper)).As<IItemService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new NpcMonsterDao(s.Resolve<NosSharpContext>(), _mapper)).As<INpcMonsterService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new NpcMonsterSkillDao(s.Resolve<NosSharpContext>(), _mapper)).As<INpcMonsterSkillService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new MapDao(s.Resolve<NosSharpContext>(), _mapper)).As<IMapService>().InstancePerLifetimeScope();

            Container.Builder.Register(s => new AccountDao(s.Resolve<NosSharpContext>(), _mapper)).As<IAccountService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new CharacterDao(s.Resolve<NosSharpContext>(), _mapper, _characterConf)).As<ICharacterService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new CharacterMateDao(s.Resolve<NosSharpContext>(), _mapper)).As<ICharacterMateService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new CharacterItemDao(s.Resolve<NosSharpContext>(), _mapper)).As<IItemInstanceService>().InstancePerLifetimeScope();

            Container.Builder.Register(s => new MapMonsterDao(s.Resolve<NosSharpContext>(), _mapper)).As<IMapMonsterService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new MapNpcDao(s.Resolve<NosSharpContext>(), _mapper)).As<IMapNpcService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new MapPortalDao(s.Resolve<NosSharpContext>(), _mapper)).As<IPortalService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new ShopDao(s.Resolve<NosSharpContext>(), _mapper)).As<IShopService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new ShopItemDao(s.Resolve<NosSharpContext>(), _mapper)).As<IShopItemService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new RecipeDao(s.Resolve<NosSharpContext>(), _mapper)).As<IRecipeService>().InstancePerLifetimeScope();
            Container.Builder.Register(s => new RecipeItemDao(s.Resolve<NosSharpContext>(), _mapper)).As<IRecipeItemService>().InstancePerLifetimeScope();
        }

        private bool Initialize()
        {

            using (var context = new NosSharpContext(new DbContextOptionsBuilder<NosSharpContext>().UseSqlServer(_configuration.ToString()).Options))
            {
                try
                {
                    context.Database.Migrate();
                    Log.Info("DATABASE_INITIALIZED");
                }
                catch (Exception ex)
                {
                    Log.Error("Database Error", ex);
                    return false;
                }

                return true;
            }
        }

        public void OnLoad()
        {
            Log.Info($"Loading...");
            _configuration = ConfigurationHelper.Load<DatabaseConfiguration>(_configurationFilePath, true); // database configuration
            _characterConf = new CharacterDto
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
            if (!Initialize())
            {
                return;
            }

            Container.Builder.Register(s => new NosSharpContext(new DbContextOptionsBuilder<NosSharpContext>().UseSqlServer(_configuration.ToString()).Options)).As<NosSharpContext>()
                .InstancePerLifetimeScope();
            RegisterMapping();
            RegisterDependencies();
            Log.Info($"Loaded !");
        }

        public void ReloadConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveConfig()
        {
            throw new NotImplementedException();
        }

        public void SaveDefaultConfig()
        {
            throw new NotImplementedException();
        }


        public void OnDisable()
        {
        }

        public void OnEnable()
        {
        }
    }
}