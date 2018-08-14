using System;
using ChickenAPI.Game.Packets.Game.Server;

namespace ChickenAPI.Game.Entities.Player.Extensions
{
    public static class FdPacketExtension
    {
        public static int GetDignityIcon(this IPlayerEntity character)
        {
            if (character.Character.Dignity <= -100)
            {
                return -2;
            }

            if (character.Character.Dignity <= -200)
            {
                return -3;
            }

            if (character.Character.Dignity <= -400)
            {
                return -4;
            }

            if (character.Character.Dignity <= -600)
            {
                return -5;
            }

            if (character.Character.Dignity <= -800)
            {
                return -6;
            }

            return -1;
        }

        public static int GetReputIcon(this IPlayerEntity player)
        {
            /*
            if (player >= 5000001)
            {
                switch (IsReputHero())
                {
                    case 1:
                        return 28;

                    case 2:
                        return 29;

                    case 3:
                        return 30;

                    case 4:
                        return 31;

                    case 5:
                        return 32;
                }
            }
            */

            if (player.Character.Reput <= 50)
            {
                return 1;
            }

            if (player.Character.Reput <= 150)
            {
                return 2;
            }

            if (player.Character.Reput <= 250)
            {
                return 3;
            }

            if (player.Character.Reput <= 500)
            {
                return 4;
            }

            if (player.Character.Reput <= 750)
            {
                return 5;
            }

            if (player.Character.Reput <= 1000)
            {
                return 6;
            }

            if (player.Character.Reput <= 2250)
            {
                return 7;
            }

            if (player.Character.Reput <= 3500)
            {
                return 8;
            }

            if (player.Character.Reput <= 5000)
            {
                return 9;
            }

            if (player.Character.Reput <= 9500)
            {
                return 10;
            }

            if (player.Character.Reput <= 19000)
            {
                return 11;
            }

            if (player.Character.Reput <= 25000)
            {
                return 12;
            }

            if (player.Character.Reput <= 40000)
            {
                return 13;
            }

            if (player.Character.Reput <= 60000)
            {
                return 14;
            }

            if (player.Character.Reput <= 85000)
            {
                return 15;
            }

            if (player.Character.Reput <= 115000)
            {
                return 16;
            }

            if (player.Character.Reput <= 150000)
            {
                return 17;
            }

            if (player.Character.Reput <= 190000)
            {
                return 18;
            }

            if (player.Character.Reput <= 235000)
            {
                return 19;
            }

            if (player.Character.Reput <= 285000)
            {
                return 20;
            }

            if (player.Character.Reput <= 350000)
            {
                return 21;
            }

            if (player.Character.Reput <= 500000)
            {
                return 22;
            }

            if (player.Character.Reput <= 1500000)
            {
                return 23;
            }

            if (player.Character.Reput <= 2500000)
            {
                return 24;
            }

            if (player.Character.Reput <= 3750000)
            {
                return 25;
            }

            return player.Character.Reput <= 5000000 ? 26 : 27;
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