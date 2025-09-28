using Assets.C_.common;
using Assets.C_.player.bag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.C_.monster
{
    // 怪物抽象类（或接口）
    public abstract class MonsterBase
    {
        public abstract int Blood { get; protected set; }
        public abstract int Attack { get; protected set; }
        public abstract int HP { get; protected set; }
        public abstract int DefensivePower { get; protected set; }
        public abstract int Speed { get; protected set; }
        public abstract List<PileOfItem> FallenObjects { get; protected set; }

        public abstract void TakeDamage(int damage);
        public abstract bool IsDead();
    }
}
