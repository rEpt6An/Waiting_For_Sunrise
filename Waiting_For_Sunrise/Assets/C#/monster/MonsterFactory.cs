using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.C_.monster
{
    public static class MonsterFactory
    {
        public enum MonsterType { Normal, Elite, Boss }

        public static MonsterBase CreateMonster(MonsterType type)
        {
            switch (type)
            {
                case MonsterType.Normal:
                    return new NormalMonster();
                case MonsterType.Elite:
                    return new EliteMonster();
                case MonsterType.Boss:
                    return new BossMonster();
                default:
                    throw new ArgumentException("Unknown monster type");
            }
        }
    }
}
