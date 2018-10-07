using System;
using Autofac;
using AutoMapper;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Core.Plugins;
using ChickenAPI.Core.Utils;
using ChickenAPI.Data.Character;
using ChickenAPI.Enums.Game.Character;
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
using SaltyEmu.DatabasePlugin.Configuration;
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

namespace SaltyEmu.DatabasePlugin
{
    public class NosSharpDatabasePlugin : IPlugin
    {
        private static readonly Logger Log = Logger.GetLogger<NosSharpDatabasePlugin>();
        private static IMapper _mapper;
        private static CharacterDto _characterConf;
        private readonly string _characterConfPath = $"plugins/config/{nameof(NosSharpDatabasePlugin)}/character_on_creation.json";
        private readonly string _configurationFilePath = $"plugins/config/{nameof(NosSharpDatabasePlugin)}/conf.json";
        private DatabaseConfiguration _configuration;
        public string Name => nameof(NosSharpDatabasePlugin);

        public void OnLoad()
        {
            Log.Info("Loading...");
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

            ChickenContainer.Builder.Register(s =>
            {
                DbContextOptionsBuilder<NosSharpContext> options = new DbContextOptionsBuilder<NosSharpContext>().UseSqlServer(_configuration.ToString());

                return new NosSharpContext(options.Options);
            }).As<NosSharpContext>().InstancePerDependency();
            RegisterMapping();
            RegisterDependencies();
            Log.Info("Loaded !");
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

        private static void RegisterMapping()
        {
            _mapper = NosSharpDatabasePluginMapper.ConfigureMapper().CreateMapper();
        }

        private static void RegisterDependencies()
        {
            // data
            ChickenContainer.Builder.Register(s => new SkillDao(s.Resolve<NosSharpContext>(), _mapper)).As<ISkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new BCardDao(s.Resolve<NosSharpContext>(), _mapper)).As<IBCardService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CardDao(s.Resolve<NosSharpContext>(), _mapper)).As<ICardService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ItemDao(s.Resolve<NosSharpContext>(), _mapper)).As<IItemService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new NpcMonsterDao(s.Resolve<NosSharpContext>(), _mapper)).As<INpcMonsterService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new NpcMonsterSkillDao(s.Resolve<NosSharpContext>(), _mapper)).As<INpcMonsterSkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapDao(s.Resolve<NosSharpContext>(), _mapper)).As<IMapService>().InstancePerLifetimeScope();

            ChickenContainer.Builder.Register(s => new AccountDao(s.Resolve<NosSharpContext>(), _mapper)).As<IAccountService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterDao(s.Resolve<NosSharpContext>(), _mapper, _characterConf)).As<ICharacterService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterMateDao(s.Resolve<NosSharpContext>(), _mapper)).As<ICharacterMateService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterItemDao(s.Resolve<NosSharpContext>(), _mapper)).As<IItemInstanceService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterSkillDao(s.Resolve<NosSharpContext>(), _mapper)).As<ICharacterSkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new CharacterQuickListDao(s.Resolve<NosSharpContext>(), _mapper)).As<ICharacterQuickListService>().InstancePerLifetimeScope();

            ChickenContainer.Builder.Register(s => new MapMonsterDao(s.Resolve<NosSharpContext>(), _mapper)).As<IMapMonsterService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapNpcDao(s.Resolve<NosSharpContext>(), _mapper)).As<IMapNpcService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new MapPortalDao(s.Resolve<NosSharpContext>(), _mapper)).As<IPortalService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ShopDao(s.Resolve<NosSharpContext>(), _mapper)).As<IShopService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ShopItemDao(s.Resolve<NosSharpContext>(), _mapper)).As<IShopItemService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new ShopSkillDao(s.Resolve<NosSharpContext>(), _mapper)).As<IShopSkillService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new RecipeDao(s.Resolve<NosSharpContext>(), _mapper)).As<IRecipeService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new RecipeItemDao(s.Resolve<NosSharpContext>(), _mapper)).As<IRecipeItemService>().InstancePerLifetimeScope();
            ChickenContainer.Builder.Register(s => new DropDao(s.Resolve<NosSharpContext>(), _mapper)).As<IDropService>().InstancePerLifetimeScope();
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
    }
}