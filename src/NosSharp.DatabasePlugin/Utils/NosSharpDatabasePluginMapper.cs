using System;
using Autofac;
using AutoMapper;
using ChickenAPI.Data.AccessLayer.NpcMonster;
using ChickenAPI.Data.TransferObjects.BCard;
using ChickenAPI.Data.TransferObjects.Character;
using ChickenAPI.Data.TransferObjects.Drop;
using ChickenAPI.Data.TransferObjects.Item;
using ChickenAPI.Data.TransferObjects.Map;
using ChickenAPI.Data.TransferObjects.NpcMonster;
using ChickenAPI.Data.TransferObjects.Skills;
using ChickenAPI.Enums.Game.BCard;
using ChickenAPI.Enums.Game.Drop;
using ChickenAPI.Utils;
using NosSharp.DatabasePlugin.Models;
using NosSharp.DatabasePlugin.Models.BCard;
using NosSharp.DatabasePlugin.Models.Character;
using NosSharp.DatabasePlugin.Models.Drops;
using NosSharp.DatabasePlugin.Models.Map;
using NosSharp.DatabasePlugin.Models.NpcMonster;
using NosSharp.DatabasePlugin.Models.Skill;

namespace NosSharp.DatabasePlugin.Utils
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
                ConfigureMapObjects(cfg);
                ConfigureCards(cfg);
            });
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
                .ForSourceMember(s => s.Npcs, expr => expr.Ignore())
                .ForSourceMember(s => s.Monsters, expr => expr.Ignore())
                .ForSourceMember(s => s.Portals, expr => expr.Ignore());

            cfg.CreateMap<MapNpcDto, MapNpcModel>()
                .ForMember(s => s.NpcMonster, expr => expr.Ignore())
                .ForMember(s => s.Map, expr => expr.Ignore());

            cfg.CreateMap<MapNpcModel, MapNpcDto>()
                .ForSourceMember(s => s.NpcMonster, expr => expr.Ignore())
                .ForMember(s => s.NpcMonster, expr => expr.ResolveUsing(origin => Container.Instance.Resolve<INpcMonsterService>().GetById(origin.NpcMonsterId)));

            cfg.CreateMap<PortalDto, MapPortalModel>()
                .ForMember(s => s.SourceMap, expr => expr.Ignore());

            cfg.CreateMap<MapPortalModel, PortalDto>()
                .ForSourceMember(s => s.SourceMap, expr => expr.Ignore());

            cfg.CreateMap<MapMonsterDto, MapMonsterModel>()
                .ForMember(s => s.NpcMonster, expr => expr.Ignore());
            cfg.CreateMap<MapMonsterModel, MapMonsterDto>()
                .ForMember(s => s.NpcMonster, expr => expr.ResolveUsing(origin => Container.Instance.Resolve<INpcMonsterService>().GetById(origin.NpcMonsterId)));
        }

        private static void MapNpcMonster(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<NpcMonsterDto, NpcMonsterModel>();
            cfg.CreateMap<NpcMonsterModel, NpcMonsterDto>()
                .ForSourceMember(s => s.BCards, expression => expression.Ignore())
                .ForSourceMember(s => s.Skills, expression => expression.Ignore())
                .ForSourceMember(s => s.MapNpcMonsters, expression => expression.Ignore());

            cfg.CreateMap<NpcMonsterSkillDto, NpcMonsterSkillModel>();
            cfg.CreateMap<NpcMonsterSkillModel, NpcMonsterSkillDto>()
                .ForSourceMember(s => s.NpcMonster, expression => expression.Ignore())
                .ForSourceMember(s => s.Skill, expr => expr.Ignore());
        }

        private static void MapBCards(IMapperConfigurationExpression cfg)
        {
            var subConfig = new MapperConfiguration(cfgg =>
            {
                cfg.CreateMap<BCardDto, BCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.Ignore());

                cfg.CreateMap<BCardDto, ItemBCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.Ignore())
                    .IncludeBase<BCardDto, BCardModel>();


                cfg.CreateMap<BCardDto, NpcMonsterBCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.Ignore())
                    .IncludeBase<BCardDto, BCardModel>();


                cfg.CreateMap<BCardDto, SkillBCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.Ignore())
                    .IncludeBase<BCardDto, BCardModel>();


                cfg.CreateMap<BCardDto, CardBCardModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.Ignore())
                    .IncludeBase<BCardDto, BCardModel>();
            });
            IMapper subMapper = subConfig.CreateMapper();
            cfg.CreateMap<BCardDto, BCardModel>()
                .ForSourceMember(s => s.RelationType, expr => expr.Ignore())
                .ConstructUsing(s =>
                {
                    switch (s.RelationType)
                    {
                        case BCardRelationType.NpcMonster:
                            return subMapper.Map<NpcMonsterBCardModel>(s);
                        case BCardRelationType.Item:
                            return subMapper.Map<ItemBCardModel>(s);
                        case BCardRelationType.Skill:
                            return subMapper.Map<SkillBCardModel>(s);
                        case BCardRelationType.Card:
                            return subMapper.Map<CardBCardModel>(s);
                        case BCardRelationType.Global:
                            return subMapper.Map<BCardModel>(s);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });

            cfg.CreateMap<BCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expr => expr.UseValue(BCardRelationType.Global));

            cfg.CreateMap<CardBCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expression => expression.UseValue(BCardRelationType.Card))
                .IncludeBase<BCardModel, BCardDto>();

            cfg.CreateMap<ItemBCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expression => expression.UseValue(BCardRelationType.Item))
                .IncludeBase<BCardModel, BCardDto>();

            cfg.CreateMap<SkillBCardModel, BCardDto>()
                .ForMember(s => s.RelationType, expression => expression.UseValue(BCardRelationType.Skill))
                .IncludeBase<BCardModel, BCardDto>();
        }

        private static void MapItems(IMapperConfigurationExpression cfg)
        {
            // Dto -> Model
            // Model -> Dto
            cfg.CreateMap<ItemDto, ItemModel>()
                .ForMember(s => s.BCards, expr => expr.Ignore());
            cfg.CreateMap<ItemModel, ItemDto>()
                .ForSourceMember(s => s.BCards, expr => expr.Ignore());

            cfg.CreateMap<ItemInstanceDto, CharacterItemModel>();
            cfg.CreateMap<CharacterItemModel, ItemInstanceDto>()
                .ForMember(s => s.Item, expr => expr.ResolveUsing(origin => Container.Instance.Resolve<INpcMonsterService>().GetById(origin.ItemId)));
        }

        private static void MapCharacters(IMapperConfigurationExpression cfg)
        {
            // Dto -> Model
            // Model -> Dto
            cfg.CreateMap(typeof(CharacterDto), typeof(CharacterModel));
            cfg.CreateMap(typeof(CharacterModel), typeof(CharacterDto));

            // Dto -> Model
            // Model -> Dto
            cfg.CreateMap(typeof(CharacterMateDto), typeof(CharacterMateModel));
            cfg.CreateMap(typeof(CharacterMateModel), typeof(CharacterMateDto));

            // Dto -> Model
            // Model -> Dto
            cfg.CreateMap(typeof(CharacterSkillDto), typeof(CharacterSkillModel));
            cfg.CreateMap(typeof(CharacterSkillModel), typeof(CharacterSkillDto));
        }

        private static void MappDrops(IProfileExpression cfg)
        {
            var subConfig = new MapperConfiguration(cfgg =>
            {
                cfg.CreateMap<DropDto, DropModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.Ignore());

                cfg.CreateMap<DropDto, MapDropModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.Ignore())
                    .IncludeBase<DropDto, DropModel>();


                cfg.CreateMap<DropDto, NpcMonsterDropModel>()
                    .ForSourceMember(src => src.RelationType, expression => expression.Ignore())
                    .IncludeBase<DropDto, DropModel>();
            });


            IMapper subMapper = subConfig.CreateMapper();
            cfg.CreateMap<DropDto, DropModel>()
                .ForSourceMember(s => s.RelationType, expr => expr.Ignore())
                .ConstructUsing(s =>
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
                .ForSourceMember(s => s.NpcMonster, expression => expression.Ignore())
                .ForMember(s => s.RelationType, expr => expr.UseValue(DropType.NpcMonster));

            cfg.CreateMap<MapDropModel, DropDto>()
                .ForSourceMember(s => s.Map, expression => expression.Ignore())
                .ForMember(s => s.RelationType, expr => expr.UseValue(DropType.MapType));

            cfg.CreateMap<DropModel, DropDto>()
                .ForMember(s => s.RelationType, expr => expr.UseValue(DropType.Global));
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
    }
}