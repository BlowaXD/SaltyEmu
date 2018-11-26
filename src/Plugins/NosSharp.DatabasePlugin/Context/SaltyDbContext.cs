using Microsoft.EntityFrameworkCore;
using SaltyEmu.DatabasePlugin.Models;
using SaltyEmu.DatabasePlugin.Models.BCard;
using SaltyEmu.DatabasePlugin.Models.Cards;
using SaltyEmu.DatabasePlugin.Models.Character;
using SaltyEmu.DatabasePlugin.Models.Families;
using SaltyEmu.DatabasePlugin.Models.Item;
using SaltyEmu.DatabasePlugin.Models.Map;
using SaltyEmu.DatabasePlugin.Models.NpcMonster;
using SaltyEmu.DatabasePlugin.Models.Shop;
using SaltyEmu.DatabasePlugin.Models.Skill;

namespace SaltyEmu.DatabasePlugin.Context
{
    public class SaltyDbContext : DbContext
    {
        public SaltyDbContext(DbContextOptions<SaltyDbContext> options) : base(options)
        {
        }

        public DbSet<AccountModel> Accounts { get; set; }

        #region Data

        public DbSet<MapModel> Maps { get; set; }

        public DbSet<SkillModel> Skills { get; set; }
        public DbSet<SkillBCardModel> SkillBCards { get; set; }
        public DbSet<CharacterQuicklistModel> QuickList { get; set; }

        public DbSet<CardModel> Cards { get; set; }
        public DbSet<CardBCardModel> CardBCards { get; set; }

        public DbSet<ItemModel> Items { get; set; }
        public DbSet<ItemBCardModel> ItemBCards { get; set; }

        public DbSet<NpcMonsterModel> NpcMonster { get; set; }
        public DbSet<NpcMonsterBCardModel> NpcMonsterBCards { get; set; }

        #endregion

        #region Character

        public DbSet<CharacterModel> Characters { get; set; }

        public DbSet<CharacterItemModel> CharacterItems { get; set; }

        public DbSet<CharacterItemOptionModel> CharacterItemsOptions { get; set; }

        public DbSet<CharacterMateModel> CharacterMates { get; set; }

        public DbSet<CharacterSkillModel> CharacterSkills { get; set; }

        public DbSet<CharacterFamilyModel> CharacterFamily { get; set; }

        #endregion

        public DbSet<FamilyModel> Families { get; set; }

        #region MapObjects

        public DbSet<MapMonsterModel> MapNpcMonsters { get; set; }

        public DbSet<MapPortalModel> MapPortals { get; set; }

        #endregion

        #region Shop

        public DbSet<ShopModel> Shops { get; set; }

        public DbSet<ShopItemModel> ShopItems { get; set; }

        public DbSet<ShopItemModel> ShopSkills { get; set; }

        public DbSet<RecipeModel> Recipes { get; set; }

        public DbSet<RecipeItemModel> RecipeItems { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Data

            modelBuilder.Entity<MapModel>()
                .HasMany(s => s.Drops)
                .WithOne(s => s.Map)
                .HasForeignKey(s => s.TypedId);

            modelBuilder.Entity<ItemModel>()
                .HasMany(s => s.BCards)
                .WithOne(s => s.Item)
                .HasForeignKey(s => s.RelationId);

            modelBuilder.Entity<SkillModel>()
                .HasMany(s => s.BCards)
                .WithOne(b => b.Skill)
                .HasForeignKey(s => s.RelationId);

            modelBuilder.Entity<CardModel>()
                .HasMany(s => s.BCards)
                .WithOne(b => b.Card)
                .HasForeignKey(s => s.RelationId);

            modelBuilder.Entity<NpcMonsterModel>()
                .HasMany(s => s.Skills)
                .WithOne(s => s.NpcMonster)
                .HasForeignKey(s => s.NpcMonsterId);

            modelBuilder.Entity<NpcMonsterModel>()
                .HasMany(s => s.BCards)
                .WithOne(s => s.NpcMonster)
                .HasForeignKey(s => s.RelationId);

            modelBuilder.Entity<NpcMonsterModel>()
                .HasMany(s => s.Drops)
                .WithOne(s => s.NpcMonster)
                .HasForeignKey(s => s.TypedId);

            modelBuilder.Entity<NpcMonsterSkillModel>()
                .HasOne(s => s.NpcMonster)
                .WithMany(s => s.Skills)
                .HasForeignKey(s => s.NpcMonsterId);

            modelBuilder.Entity<NpcMonsterSkillModel>()
                .HasOne(s => s.Skill)
                .WithMany(s => s.NpcMonsterSkills)
                .HasForeignKey(s => s.SkillId);

            #endregion

            #region Account

            modelBuilder.Entity<AccountModel>()
                .HasMany(s => s.Characters)
                .WithOne(s => s.Account);

            #endregion

            #region Character

            modelBuilder.Entity<CharacterModel>()
                .HasOne(s => s.Account)
                .WithMany(s => s.Characters);

            modelBuilder.Entity<CharacterMateModel>()
                .HasOne(s => s.Character)
                .WithMany(s => s.Mates)
                .HasForeignKey(s => s.CharacterId);

            modelBuilder.Entity<CharacterMateModel>()
                .HasOne(s => s.NpcMonster)
                .WithMany(s => s.CharacterMates)
                .HasForeignKey(s => s.NpcMonsterId);

            modelBuilder.Entity<CharacterSkillModel>()
                .HasOne(s => s.Character)
                .WithMany(s => s.Skills)
                .HasForeignKey(s => s.CharacterId);

            modelBuilder.Entity<CharacterQuicklistModel>()
                .HasOne(s => s.Character)
                .WithMany(s => s.Quicklist)
                .HasForeignKey(s => s.CharacterId);

            modelBuilder.Entity<CharacterItemModel>()
                .HasOne(s => s.Item)
                .WithMany(s => s.CharacterItems)
                .HasForeignKey(s => s.ItemId);

            modelBuilder.Entity<CharacterItemModel>()
                .HasOne(s => s.Character)
                .WithMany(s => s.Items)
                .HasForeignKey(s => s.CharacterId);

            modelBuilder.Entity<CharacterItemModel>()
                .HasOne(s => s.BoundCharacterModel)
                .WithMany(s => s.BoundItems)
                .HasForeignKey(s => s.BoundCharacterId);

            modelBuilder.Entity<CharacterItemOptionModel>()
                .HasOne(s => s.CharacterItem)
                .WithMany(s => s.ItemOptions)
                .HasForeignKey(s => s.WearableInstanceId);

            modelBuilder.Entity<CharacterItemModel>()
                .HasMany(s => s.ItemOptions)
                .WithOne(s => s.CharacterItem)
                .HasForeignKey(s => s.WearableInstanceId);

            modelBuilder.Entity<CharacterFamilyModel>()
                .HasOne(s => s.Family)
                .WithMany(s => s.FamilyMembers)
                .HasForeignKey(s => s.FamilyId);

            #endregion

            modelBuilder.Entity<FamilyModel>()
                .HasMany(s => s.FamilyMembers)
                .WithOne(s => s.Family)
                .HasForeignKey(s => s.FamilyId);

            #region MapItems 

            modelBuilder.Entity<MapMonsterModel>()
                .HasOne(s => s.NpcMonster)
                .WithMany(s => s.MapNpcMonsters)
                .HasForeignKey(s => s.NpcMonsterId);

            modelBuilder.Entity<MapPortalModel>()
                .HasOne(s => s.SourceMap)
                .WithMany(s => s.Portals)
                .HasForeignKey(s => s.SourceMapId);

            modelBuilder.Entity<MapModel>()
                .HasMany(s => s.Npcs)
                .WithOne(s => s.Map)
                .HasForeignKey(s => s.MapId);

            modelBuilder.Entity<MapModel>()
                .HasMany(s => s.Monsters)
                .WithOne(s => s.Map)
                .HasForeignKey(s => s.MapId);

            #endregion

            #region Recipes And Shops

            modelBuilder.Entity<ShopModel>()
                .HasOne(s => s.MapNpc)
                .WithOne(s => s.Shop)
                .HasForeignKey<ShopModel>(s => s.MapNpcId);

            modelBuilder.Entity<ShopItemModel>()
                .HasOne(s => s.Item)
                .WithMany(s => s.ShopItems)
                .HasForeignKey(s => s.ItemId);

            modelBuilder.Entity<ShopItemModel>()
                .HasOne(s => s.Shop)
                .WithMany(s => s.ShopItems)
                .HasForeignKey(s => s.ShopId);

            modelBuilder.Entity<ShopSkillModel>()
                .HasOne(s => s.Shop)
                .WithMany(s => s.ShopSkills)
                .HasForeignKey(s => s.ShopId);

            modelBuilder.Entity<RecipeModel>()
                .HasOne(s => s.Shop)
                .WithMany(s => s.Recipes)
                .HasForeignKey(s => s.ShopId);

            modelBuilder.Entity<RecipeModel>()
                .HasMany(s => s.RecipeItems)
                .WithOne(s => s.Recipe)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipeItemModel>()
                .HasOne(s => s.Item)
                .WithMany(s => s.RecipeItems)
                .HasForeignKey(s => s.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}