using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Maths;
using ChickenAPI.Data.Skills;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Effects;
using ChickenAPI.Game.Entities.Extensions;
using ChickenAPI.Game.Entities.Player;
using ChickenAPI.Game.Entities.Player.Extensions;
using ChickenAPI.Game.Families.Extensions;
using ChickenAPI.Game.Inventory.Extensions;
using ChickenAPI.Game.Movements.Extensions;
using ChickenAPI.Game.Skills;
using ChickenAPI.Game.Skills.Extensions;
using ChickenAPI.Packets.Game.Client.Player;
using ChickenAPI.Packets.Game.Server.Player;
using ChickenAPI.Packets.Game.Server.UserInterface;
using System;

namespace ChickenAPI.Game.Player.Extension
{
    public static class PlayerExtension
    {
        private static readonly IRandomGenerator _randomGenerator = new Lazy<IRandomGenerator>(() => ChickenContainer.Instance.Resolve<IRandomGenerator>()).Value;

        #region Gold

        public static void GoldLess(this IPlayerEntity charac, long amount)
        {
            charac.Character.Gold -= amount;
            charac.SendPacket(charac.GenerateGoldPacket());
        }

        public static void GoldUp(this IPlayerEntity charac, long amount)
        {
            charac.Character.Gold += amount;
            charac.SendPacket(charac.GenerateGoldPacket());
        }

        #endregion Gold

        public static void ChangeClass(this IPlayerEntity charac, CharacterClassType type)
        {
            charac.Level = 1;
            charac.JobLevelXp = 0;
            charac.SendPacket(new NpInfoPacket { UnKnow = 0 });
            charac.SendPacket(new PClearPacket { });

            if (type == (byte)CharacterClassType.Adventurer)
            {
                charac.Character.HairStyle = (byte)charac.Character.HairStyle > 1 ? 0 : charac.Character.HairStyle;
            }

            charac.Character.Class = type;
            charac.Hp = charac.HpMax;
            charac.Mp = charac.MpMax;
            charac.SendPacket(new TitPacket
            {
                ClassType = type == CharacterClassType.Adventurer ? "Adventurer" :
                type == CharacterClassType.Archer ? "Archer" :
                type == CharacterClassType.Magician ? "Mage" :
                type == CharacterClassType.Swordman ? "Swordman" : "Martial",
                Name = charac.Character.Name
            });
            charac.SendPacket(charac.GenerateStatPacket());
            charac.SendPacket(charac.GenerateEqPacket());
            charac.SendPacket(charac.GenerateEffectPacket(8));
            charac.SendPacket(charac.GenerateEffectPacket(196));
            charac.SendPacket(new ScrPacket { Unknow1 = 0, Unknow2 = 0, Unknow3 = 0, Unknow4 = 0, Unknow5 = 0, Unknow6 = 0 });
            // Session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey("CLASS_CHANGED"), 0));
            charac.Character.Faction = charac.Family == null
             ? (FactionType)(1 + _randomGenerator.Next(0, 2))
             : (FactionType)charac.Family.FamilyFaction;
            // Session.SendPacket(UserInterfaceHelper.GenerateMsg(Language.Instance.GetMessageFromKey($"GET_PROTECTION_POWER_{Faction}"), 0));*/
            charac.SendPacket(charac.GenerateFsPacket());
            charac.SendPacket(charac.GenerateStatCharPacket());
            charac.SendPacket(charac.GenerateEffectPacket((4799 + (byte)charac.Character.Faction)));
            charac.SendPacket(charac.GenerateCondPacket());
            charac.SendPacket(charac.GenerateLevPacket());
            charac.Broadcast(charac.GenerateCModePacket());
            // GenerateIn()
            charac.Broadcast(charac.GenerateGidxPacket());
            charac.Broadcast(charac.GenerateEffectPacket(6));
            charac.Broadcast(charac.GenerateEffectPacket(196));
            //Session.CurrentMapInstance?.Broadcast(Session, GenerateIn(), ReceiverType.AllExceptMe);

            SkillComponent component = charac.SkillComponent;
            foreach (SkillDto skill in component.Skills.Values)
            {
                if (skill.Id >= 200)
                {
                    component.Skills.Remove(skill.Id);
                }
            }

            // TODO : LATER ADD SKILL
            charac.SendPacket(charac.GenerateSkiPacket());

            // Later too
            /* foreach (QuicklistEntryDTO quicklists in DAOFactory.QuicklistEntryDAO.LoadByCharacterId(CharacterId).Where(quicklists => QuicklistEntries.Any(qle => qle.Id == quicklists.Id)))
             {
                 DAOFactory.QuicklistEntryDAO.Delete(quicklists.Id);
             }

             QuicklistEntries = new List<QuicklistEntryDTO>
             {
                 new QuicklistEntryDTO
                 {
                     CharacterId = CharacterId,
                     Q1 = 0,
                     Q2 = 9,
                     Type = 1,
                     Slot = 3,
                     Pos = 1
                 }
             };
             if (ServerManager.Instance.Groups.Any(s => s.IsMemberOfGroup(Session) && s.GroupType == GroupType.Group))
             {
                 Session.CurrentMapInstance?.Broadcast(Session, $"pidx 1 1.{CharacterId}", ReceiverType.AllExceptMe);
             }*/
        }
    }
}