using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Data.Item;
using ChickenAPI.Data.Skills;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Packets.Game.Server.Inventory;

namespace ChickenAPI.Game.Inventory.Extensions
{
    public static class SlInfoPacketExtensions
    {
        public static SlInfoPacket GenerateSlInfoPacket(this ItemInstanceDto itemInstance)
        {
            SkillDto[] skills = ChickenContainer.Instance.Resolve<ISkillService>().GetByClassId(itemInstance.GetClassId());

            var skillString = new StringBuilder();

            foreach (SkillDto i in skills)
            {
                skillString.Append(i.Id);
                skillString.Append('.');
            }

            return new SlInfoPacket
            {
                // TODO: Sp Points system
                // TODO: Sp SL system
                // TODO: Sp Destroyed control

                InventoryType = itemInstance.Type,
                ItemId = itemInstance.ItemId,
                ItemMorph = itemInstance.Item.Morph,
                SpLevel = itemInstance.Level,
                LevelJobMinimum = itemInstance.Item.LevelJobMinimum,
                ReputationMinimum = itemInstance.Item.ReputationMinimum,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0,
                Unknown5 = 0,
                Unknown6 = 0,
                Unknown7 = 0,
                SpType = itemInstance.Item.SpType,
                FireResistance = itemInstance.Item.FireResistance,
                WaterResistance = itemInstance.Item.WaterResistance,
                LightResistance = itemInstance.Item.LightResistance,
                DarkResistance = itemInstance.Item.DarkResistance,
                Xp = itemInstance.Xp,
                SPXpData = 0, // Sp xp calculation
                Skill = skillString.ToString().TrimEnd('.'),
                TransportId = 0, // TODO: transport system
                FreeSpPoints = 0,
                SlHit = 0,
                SlDefence = 0,
                SlElement = 0,
                SlHp = 0,
                Upgrade = itemInstance.Upgrade,
                Unknown8 = 0,
                Unknown9 = 0,
                IsSpDestroyed = 0,
                ShellHit = 0,
                ShellDefense = 0,
                ShellElement = 0,
                ShellHpMp = 0,
                SpStoneUpgrade = itemInstance.SpStoneUpgrade, // TODO: sp stone upgrade
                AttackPoints = itemInstance.AttackPoints,
                DefensePoints = itemInstance.DefensePoints,
                ElementPoints = itemInstance.ElementPoints,
                HpMpPoints = itemInstance.HpMpPoints,
                SpFireResistance = itemInstance.FireResistance,
                SpWaterResistance = itemInstance.WaterResistance,
                SpLightResistance = itemInstance.LightResistance,
                SpDarkResistance = itemInstance.DarkResistance
            };
        }
    }
}