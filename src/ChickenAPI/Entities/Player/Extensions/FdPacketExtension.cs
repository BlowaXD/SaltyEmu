using System;
using ChickenAPI.Enums.Game.Character;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class FdPacketExtension
    {
        public static int GetDignityIcon(this IPlayerEntity character)
        {
            if (character.Character.Dignity <= -100)
            {
                return (int)CharacterDignity.Suspected;
            }

            if (character.Character.Dignity <= -200)
            {
                return (int)CharacterDignity.BluffedNameOnly;
            }

            if (character.Character.Dignity <= -400)
            {
                return (int)CharacterDignity.NotQualifiedFor;
            }

            if (character.Character.Dignity <= -600)
            {
                return (int)CharacterDignity.Useless;
            }

            if (character.Character.Dignity <= -800)
            {
                return (int)CharacterDignity.StupidMinded;
            }

            return (int)CharacterDignity.Basic;
        }

        public static int GetReputIcon(this IPlayerEntity player)
        {
            /*
            if (player >= 5000001)
            {
                switch (IsReputHero())
                {
                    case 1:
                        return (int)CharacterRep.LegendG;

                    case 2:
                        return (int)CharacterRep.Legendb;

                    case 3:
                        return (int)CharacterRep.AncienHeros;

                    case 4:
                        return (int)CharacterRep.MysteriousHeros;

                    case 5:
                        return (int)CharacterRep.LegendaryHeros;
                }
            }
            */

            if (player.Character.Reput <= 50)
            {
                return (int)CharacterRep.Beginer;
            }

            if (player.Character.Reput <= 150)
            {
                return (int)CharacterRep.IDk;
            }

            if (player.Character.Reput <= 250)
            {
                return (int)CharacterRep.IDk2;
            }

            if (player.Character.Reput <= 500)
            {
                return (int)CharacterRep.TraineeG;
            }

            if (player.Character.Reput <= 750)
            {
                return (int)CharacterRep.TraineeB;
            }

            if (player.Character.Reput <= 1000)
            {
                return (int)CharacterRep.TraineeR;
            }

            if (player.Character.Reput <= 2250)
            {
                return (int)CharacterRep.TheExperiencedG;
            }

            if (player.Character.Reput <= 3500)
            {
                return (int)CharacterRep.TheExperiencedB;
            }

            if (player.Character.Reput <= 5000)
            {
                return (int)CharacterRep.TheExperiencedR;
            }

            if (player.Character.Reput <= 9500)
            {
                return (int)CharacterRep.BattleSoldierG;
            }

            if (player.Character.Reput <= 19000)
            {
                return (int)CharacterRep.BattleSoldierB;
            }

            if (player.Character.Reput <= 25000)
            {
                return (int)CharacterRep.BattleSoldierR;
            }

            if (player.Character.Reput <= 40000)
            {
                return (int)CharacterRep.ExpertG;
            }

            if (player.Character.Reput <= 60000)
            {
                return (int)CharacterRep.ExpertB;
            }

            if (player.Character.Reput <= 85000)
            {
                return (int)CharacterRep.ExpertR;
            }

            if (player.Character.Reput <= 115000)
            {
                return (int)CharacterRep.LeaderG;
            }

            if (player.Character.Reput <= 150000)
            {
                return (int)CharacterRep.LeaderB;
            }

            if (player.Character.Reput <= 190000)
            {
                return (int)CharacterRep.LeaderR;
            }

            if (player.Character.Reput <= 235000)
            {
                return (int)CharacterRep.MasterG;
            }

            if (player.Character.Reput <= 285000)
            {
                return (int)CharacterRep.MasterB;
            }

            if (player.Character.Reput <= 350000)
            {
                return (int)CharacterRep.MasterR;
            }

            if (player.Character.Reput <= 500000)
            {
                return (int)CharacterRep.NosG;
            }

            if (player.Character.Reput <= 1500000)
            {
                return (int)CharacterRep.NosB;
            }

            if (player.Character.Reput <= 2500000)
            {
                return (int)CharacterRep.NosR;
            }

            if (player.Character.Reput <= 3750000)
            {
                return (int)CharacterRep.EliteG;
            }

            return player.Character.Reput <= 5000000 ? (int)CharacterRep.EliteB : (int)CharacterRep.EliteR;
        }

        public static FdPacket GenerateFdPacket(this IPlayerEntity player)
        {
            return new FdPacket
            {
                Dignity = (int)player.Character.Dignity,
                Reput = player.Character.Reput,
                DignityIcon = Math.Abs(player.GetDignityIcon()),
                ReputIcon = player.GetReputIcon(),
            };
        }
    }
}