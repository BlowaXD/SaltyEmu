using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using ChickenAPI.Core.IoC;
using ChickenAPI.Core.Logging;
using ChickenAPI.Data.NpcMonster;

namespace Toolkit.Converter
{
    public class EInfoFiller
    {
        private static readonly Logger Log = Logger.GetLogger<EInfoFiller>();
        private readonly List<NpcMonsterDto> _npcMonsters = new List<NpcMonsterDto>();
        private INpcMonsterService _npcMonsterService;

        public void Fill(string filePath)
        {
            _npcMonsterService = ChickenContainer.Instance.Resolve<INpcMonsterService>();
            string tmp = File.ReadAllText(filePath, Encoding.GetEncoding(1252));
            string[] lines = tmp.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (string line in lines.Where(s => s.StartsWith("e_info 10")))
            {
                string[] currentPacket = line.Split(new[] { ' ', '\t' }, StringSplitOptions.None);

                if (currentPacket.Length <= 25)
                {
                    continue;
                }

                NpcMonsterDto npcMonster = _npcMonsterService.GetById(short.Parse(currentPacket[2]));
                if (npcMonster == null)
                {
                    continue;
                }

                npcMonster.AttackClass = byte.Parse(currentPacket[5]);
                npcMonster.AttackUpgrade = byte.Parse(currentPacket[7]);
                npcMonster.DamageMinimum = short.Parse(currentPacket[8]);
                npcMonster.DamageMaximum = short.Parse(currentPacket[9]);
                npcMonster.Concentrate = short.Parse(currentPacket[10]);
                npcMonster.CriticalChance = byte.Parse(currentPacket[11]);
                npcMonster.CriticalRate = short.Parse(currentPacket[12]);
                npcMonster.DefenceUpgrade = byte.Parse(currentPacket[13]);
                npcMonster.CloseDefence = short.Parse(currentPacket[14]);
                npcMonster.DefenceDodge = short.Parse(currentPacket[15]);
                npcMonster.DistanceDefence = short.Parse(currentPacket[16]);
                npcMonster.DistanceDefenceDodge = short.Parse(currentPacket[17]);
                npcMonster.MagicDefence = short.Parse(currentPacket[18]);
                npcMonster.FireResistance = sbyte.Parse(currentPacket[19]);
                npcMonster.WaterResistance = sbyte.Parse(currentPacket[20]);
                npcMonster.LightResistance = sbyte.Parse(currentPacket[21]);
                npcMonster.DarkResistance = sbyte.Parse(currentPacket[22]);
                if (_npcMonsters.Any(s => s.Id == npcMonster.Id))
                {
                    continue;
                }

                _npcMonsters.Add(npcMonster);
            }

            _npcMonsterService.Save(_npcMonsters);
        }
    }
}