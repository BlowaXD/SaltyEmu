using System;
using Autofac;
using AutoMapper;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Account;
using ChickenAPI.Data.BCard;
using ChickenAPI.Data.Character;
using ChickenAPI.Data.Drop;
using ChickenAPI.Data.Families;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Map;
using ChickenAPI.Data.NpcMonster;
using ChickenAPI.Data.Shop;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Enums.Game.Drop;
using SaltyEmu.DatabasePlugin.Models.BCard;
using SaltyEmu.DatabasePlugin.Models.Cards;
using SaltyEmu.DatabasePlugin.Models.Character;
using SaltyEmu.DatabasePlugin.Models.Drops;
using SaltyEmu.DatabasePlugin.Models.Families;
using SaltyEmu.DatabasePlugin.Models.Item;
using SaltyEmu.DatabasePlugin.Models.Map;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;
using SaltyEmu.DatabasePlugin.Models.Shop;
using SaltyEmu.DatabasePlugin.Models.Skill;

namespace SaltyEmu.DatabasePlugin.Utils
{
    public class NosSharpDatabasePluginMapper
    {
        public static MapperConfiguration ConfigureMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                MappDrops(cfg);
                MappSkills(cfg);
                MapAccounts(cfg);
                MapCharacters(cfg);
                MapItems(cfg);
                MapBCards(cfg);
                MapNpcMonster(cfg);
                MapShop(cfg);
                ConfigureFamilies(cfg);
                ConfigureMapObjects(cfg);
                ConfigureCards(cfg);
            });
        }

        private static void ConfigureFamilies(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<FamilyModel, FamilyDto>();
            cfg.CreateMap<FamilyDto, FamilyModel>();
        }

        private static void ConfigureCards(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CardDto, CardModel>();
            cfg.CreateMap<CardModel, CardDto>();
        }

        private static void ConfigureMapObjects(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<MapDto, MapModel>();
            cfg.CreateMap<MapModel, MapDto>()
                .ForSourceMember(s => s.Npcs, expr => expr.DoNotValidate())
                .ForSourceMember(s => s.Monsters, expr => expr.DoNotValidate())
                .ForSourceMember(s => s.Portals, expr => expr.DoNotValidate());

            cfg.CreateMap<MapNpcDto, MapNpcModel>()
                .ForMember(s => s.NpcMonster, expr => expr.Ignore())
                .ForMember(s => s.Map, expr => expr.Ignore());

            cfg.CreateMap<MapNpcModel, MapNpcDto>()
                .ForSourceMember(s => s.NpcMonster, expr => expr.DoNotValidate())
                .ForMember(s => s.NpcMonster, expr => expr.MapFrom(origin => ChickenContainer.Instance.Resolve<INpcMonsterService>().GetById(origin.NpcMonsterId)));

            cfg.CreateMap<PortalDto, MapPortalModel>()
                .ForMember(s => s.SourceMap, expr => expr.Ignore());

            cfg.CreateMap<MapPortalModel, PortalDto>()
                .ForSourceMember(s => s.SourceMap, expr => expr.DoNotValidate());

            cfg.CreateMap<MapMonsterDto, MapMonsterModel>()
                .ForMember(s => s.NpcMonster, expr => expr.Ignore());
            cfg.CreateMap<MapMonsterModel, MapMonsterDto>()
                .ForMember(s => s.NpcMonster, expr => expr.MapFrom(origin => ChickenContainer.Instance.Resolve<INpcMonsterService>().GetById(origin.NpcMonsterId)));
        }

        private static void MapNpcMonster(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<NpcMonsterDto, NpcMonsterModel>();
            cfg.CreateMap<NpcMonsterModel, NpcMonsterDto>()
                .ForSourceMember(s => s.BCards, expression => expression.DoNotValidate())
                .ForSourceMember(s => s.Skills, expression => expression.DoNotValidate())
                .ForSourceMember(s => s.MapNpcMonsters, expression => expression.DoNotValidate());

            cfg.CreateMap<NpcMonsterSkillDto, NpcMonsterSkillModel>();
            cfg.CreateMap<NpcMonsterSkillModel, NpcMonsterSkillDto>()
                .ForSourceMember(s => s.NpcMonster, expression => expression.DoNotValidate())
                .ForSourceMember(s => s.Skill, expr => expr.DoNotValidate())
                .ForMember(s => s.Skill, expr => expr.MapFrom(origin => ChickenContainer.Instance.Resolve<ISkillService>().GetById(origin.SkillId)));
        }

        private static void MapBCards(IMapperConfigurationExpression cfg)
        {
            var subConfig = new MapperConfiguration(cfgg =>
            {
                cfg.CreateMap<BCardDto, BCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.DoNotValidate());

                cfg.CreateMap<BCardDto, ItemBCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.DoNotValidate())
                    .IncludeBase<BCardDto, BCardModel>();


                cfg.CreateMap<BCardDto, NpcMonsterBCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.DoNotValidate())
                    .IncludeBase<BCardDto, BCardModel>();


                cfg.CreateMap<BCardDto, SkillBCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.DoNotValidate())
                    .IncludeBase<BCardDto, BCardModel>();


                cfg.CreateMap<BCardDto, CardBCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.DoNotValidate())
                    .IncludeBase<BCardDto, BCardModel>();
            });
            IMapper subMapper = subConfig.CreateMapper();
            cfg.CreateMap<BCardDto, BCardModel>()
                .ForSourceMember(s => s.RelationType, expr => expr.DoNotValidate())
                .ConstructUsing((dto, context) =>
                {
                    switch (dto.RelationType)
                    {
                        case BCardRelationType.NpcMonster:
                            return subMapper.Map<NpcMonsterBCardModel>(dto);
                        case BCardRelationType.Item:
                            return subMapper.Map<ItemBCardModel>(dto);
                        case BCardRelationType.Skill:
                            return subMapper.Map<SkillBCardModel>(dto);
                        case BCardRelationType.Card:
                            return subMapper.Map<CardBCardModel>(dto);
                        case BCardRelationType.Global:
                            return subMapper.Map<BCardModel>(dto);
                        default:
                            return null;
                    }
                });

            cfg.CreateMap<BCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expr => expr.MapFrom(s => BCardRelationType.Global))
                .ForMember(s => s.Type, expression => expression.MapFrom(s => (BCardType)s.Type));

            cfg.CreateMap<CardBCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expression => expression.MapFrom(s => BCardRelationType.Card))
                .ForMember(s => s.Type, expression => expression.MapFrom(s => (BCardType)s.Type))
                .IncludeBase<BCardModel, BCardDto>();

            cfg.CreateMap<ItemBCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expression => expression.MapFrom(s => BCardRelationType.Item))
                .ForMember(s => s.Type, expression => expression.MapFrom(s => (BCardType)s.Type))
                .IncludeBase<BCardModel, BCardDto>();

            cfg.CreateMap<NpcMonsterBCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expression => expression.MapFrom(s => BCardRelationType.NpcMonster))
                .ForMember(s => s.Type, expression => expression.MapFrom(s => (BCardType)s.Type))
                .IncludeBase<BCardModel, BCardDto>();

            cfg.CreateMap<SkillBCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expression => expression.MapFrom(s => BCardRelationType.Skill))
                .ForMember(s => s.Type, expression => expression.MapFrom(s => (BCardType)s.Type))
                .IncludeBase<BCardModel, BCardDto>();
        }

        private static void MapItems(IMapperConfigurationExpression cfg)
        {
            // Dto -> Model
            // Model -> Dto
            cfg.CreateMap<ItemDto, ItemModel>()
                .ForMember(s => s.BCards, expr => expr.Ignore());
            cfg.CreateMap<ItemModel, ItemDto>()
                .ForSourceMember(s => s.BCards, expr => expr.DoNotValidate());

            cfg.CreateMap<ItemInstanceDto, CharacterItemModel>();

            cfg.CreateMap<CharacterItemModel, ItemInstanceDto>()
                .ForMember(s => s.Item, expr => expr.MapFrom(origin => ChickenContainer.Instance.Resolve<IItemService>().GetById(origin.ItemId)));
        }

        private static void MapCharacters(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CharacterDto, CharacterModel>();
            cfg.CreateMap<CharacterModel, CharacterDto>();

            cfg.CreateMap<CharacterMateDto, CharacterMateModel>();
            cfg.CreateMap<CharacterMateModel, CharacterMateDto>();

            cfg.CreateMap<CharacterSkillDto, CharacterSkillModel>()
                .ForMember(s => s.Skill, expr => expr.Ignore());

            cfg.CreateMap<CharacterSkillModel, CharacterSkillDto>()
                .ForMember(s => s.Skill, expr => expr.MapFrom(origin => ChickenContainer.Instance.Resolve<ISkillService>().GetById(origin.SkillId)));

            cfg.CreateMap<CharacterQuicklistDto, CharacterQuicklistModel>();
            cfg.CreateMap<CharacterQuicklistModel, CharacterQuicklistDto>();

            cfg.CreateMap<CharacterFamilyDto, CharacterFamilyModel>();
            cfg.CreateMap<CharacterFamilyModel, CharacterFamilyDto>();
        }

        private static void MappDrops(IProfileExpression cfg)
        {
            var subConfig = new MapperConfiguration(cfgg =>
            {
                cfg.CreateMap<DropDto, DropModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.DoNotValidate());

                cfg.CreateMap<DropDto, MapDropModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.DoNotValidate())
                    .IncludeBase<DropDto, DropModel>();


                cfg.CreateMap<DropDto, NpcMonsterDropModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.DoNotValidate())
                    .IncludeBase<DropDto, DropModel>();
            });


            IMapper subMapper = subConfig.CreateMapper();
            cfg.CreateMap<DropDto, DropModel>()
                .ForSourceMember(s => s.RelationType, expr => expr.DoNotValidate())
                .ConstructUsing((s, context) =>
                {
                    switch (s.RelationType)
                    {
                        case DropType.MapType:
                            return subMapper.Map<MapDropModel>(s);
                        case DropType.NpcMonster:
                            return subMapper.Map<NpcMonsterDropModel>(s);
                        case DropType.Global:
                            return subMapper.Map<DropModel>(s);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });


            cfg.CreateMap<NpcMonsterDropModel, DropDto>()
                .ForSourceMember(s => s.NpcMonster, expression => expression.DoNotValidate())
                .ForMember(s => s.RelationType, expr => expr.MapFrom(s => DropType.NpcMonster));

            cfg.CreateMap<MapDropModel, DropDto>()
                .ForSourceMember(s => s.Map, expression => expression.DoNotValidate())
                .ForMember(s => s.RelationType, expr => expr.MapFrom(s => DropType.MapType));

            cfg.CreateMap<DropModel, DropDto>()
                .ForMember(s => s.RelationType, expr => expr.MapFrom(s => DropType.Global));
        }

        private static void MappSkills(IMapperConfigurationExpression cfg)
        {
            // Dto -> Model
            // Model -> Dto
            cfg.CreateMap<SkillDto, SkillModel>();
            cfg.CreateMap<SkillModel, SkillDto>();
        }

        private static void MapAccounts(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AccountDto, AccountModel>();
            cfg.CreateMap<AccountModel, AccountDto>();
        }

        private static void MapShop(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ShopDto, ShopModel>();
            cfg.CreateMap<ShopModel, ShopDto>();

            cfg.CreateMap<ShopItemDto, ShopItemModel>();
            cfg.CreateMap<ShopItemModel, ShopItemDto>()
                .ForMember(s => s.Item, expression => expression.MapFrom(origin => ChickenContainer.Instance.Resolve<IItemService>().GetById(origin.ItemId)));

            cfg.CreateMap<ShopSkillDto, ShopSkillModel>();
            cfg.CreateMap<ShopSkillModel, ShopSkillDto>()
                .ForMember(s => s.Skill, expression => expression.MapFrom(origin => ChickenContainer.Instance.Resolve<ISkillService>().GetById(origin.SkillId)));

            cfg.CreateMap<RecipeDto, RecipeModel>();
            cfg.CreateMap<RecipeModel, RecipeDto>();

            cfg.CreateMap<RecipeItemDto, RecipeItemModel>();
            cfg.CreateMap<RecipeItemModel, RecipeItemDto>();
        }
    }
}