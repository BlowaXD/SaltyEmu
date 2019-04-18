using System;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Packets.Game.Server.Player;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class FdPacketExtension
    {
        public static CharacterDignity GetDignityIcon(this IPlayerEntity character)
        {
            if (character.Character.Dignity <= -100)
            {
                return CharacterDignity.Suspected;
            }

            if (character.Character.Dignity <= -200)
            {
                return CharacterDignity.BluffedNameOnly;
            }

            if (character.Character.Dignity <= -400)
            {
                return CharacterDignity.NotQualifiedFor;
            }

            if (character.Character.Dignity <= -600)
            {
                return CharacterDignity.Useless;
            }

            if (character.Character.Dignity <= -800)
            {
                return CharacterDignity.StupidMinded;
            }

            return CharacterDignity.Basic;
        }

        public static CharacterRep GetReputIcon(this IPlayerEntity player)
        {
            CharacterDignity dignity = player.GetDignityIcon();
            if (dignity != CharacterDignity.Basic)
            {
                switch (dignity)
                {
                    case CharacterDignity.Basic:
                        break;
                    case CharacterDignity.Suspected:
                        return CharacterRep.Suspected;
                    case CharacterDignity.BluffedNameOnly:
                        return CharacterRep.BluffedNameOnly;
                    case CharacterDignity.NotQualifiedFor:
                        return CharacterRep.NotQualifiedFor;
                    case CharacterDignity.Useless:
                        return CharacterRep.Useless;
                    case CharacterDignity.StupidMinded:
                        return CharacterRep.StupidMinded;
                }
            }
            /*
            if (player >= 5000001)
            {
                switch (IsReputHero())
                {
                    case 1:
                        return CharacterRep.LegendG;

                    case 2:
                        return CharacterRep.Legendb;

                    case 3:
                        return CharacterRep.AncienHeros;

                    case 4:
                        return CharacterRep.MysteriousHeros;

                    case 5:
                        return CharacterRep.LegendaryHeros;
                }
            }
            */

            if (player.Character.Reput <= 50)
            {
                return CharacterRep.Beginner;
            }

            if (player.Character.Reput <= 150)
            {
                return CharacterRep.IDk;
            }

            if (player.Character.Reput <= 250)
            {
                return CharacterRep.IDk2;
            }

            if (player.Character.Reput <= 500)
            {
                return CharacterRep.TraineeG;
            }

            if (player.Character.Reput <= 750)
            {
                return CharacterRep.TraineeB;
            }

            if (player.Character.Reput <= 1000)
            {
                return CharacterRep.TraineeR;
            }

            if (player.Character.Reput <= 2250)
            {
                return CharacterRep.TheExperiencedG;
            }

            if (player.Character.Reput <= 3500)
            {
                return CharacterRep.TheExperiencedB;
            }

            if (player.Character.Reput <= 5000)
            {
                return CharacterRep.TheExperiencedR;
            }

            if (player.Character.Reput <= 9500)
            {
                return CharacterRep.BattleSoldierG;
            }

            if (player.Character.Reput <= 19000)
            {
                return CharacterRep.BattleSoldierB;
            }

            if (player.Character.Reput <= 25000)
            {
                return CharacterRep.BattleSoldierR;
            }

            if (player.Character.Reput <= 40000)
            {
                return CharacterRep.ExpertG;
            }

            if (player.Character.Reput <= 60000)
            {
                return CharacterRep.ExpertB;
            }

            if (player.Character.Reput <= 85000)
            {
                return CharacterRep.ExpertR;
            }

            if (player.Character.Reput <= 115000)
            {
                return CharacterRep.LeaderG;
            }

            if (player.Character.Reput <= 150000)
            {
                return CharacterRep.LeaderB;
            }

            if (player.Character.Reput <= 190000)
            {
                return CharacterRep.LeaderR;
            }

            if (player.Character.Reput <= 235000)
            {
                return CharacterRep.MasterG;
            }

            if (player.Character.Reput <= 285000)
            {
                return CharacterRep.MasterB;
            }

            if (player.Character.Reput <= 350000)
            {
                return CharacterRep.MasterR;
            }

            if (player.Character.Reput <= 500000)
            {
                return CharacterRep.NosG;
            }

            if (player.Character.Reput <= 1500000)
            {
                return CharacterRep.NosB;
            }

            if (player.Character.Reput <= 2500000)
            {
                return CharacterRep.NosR;
            }

            if (player.Character.Reput <= 3750000)
            {
                return CharacterRep.EliteG;
            }

            return player.Character.Reput <= 5000000 ? CharacterRep.EliteB : CharacterRep.EliteR;
        }

        public static FdPacket GenerateFdPacket(this IPlayerEntity player)
        {
            return new FdPacket
            {
                Reput = player.Character.Reput,
                ReputIcon = player.GetDignityIcon() == CharacterDignity.Basic ? (int)player.GetReputIcon() : (int)player.GetDignityIcon(),
                Dignity = (int)player.Character.Dignity,
                DignityIcon = Math.Abs((int)player.GetDignityIcon())
            };
        }
    }
}