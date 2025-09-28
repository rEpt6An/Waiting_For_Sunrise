using Assets.C_.common;
using Assets.C_.player.bag;
using System;
using System.Collections.Generic;

namespace Assets.C_.monster
{
    public  class EliteMonster : MonsterBase
    {
        public override int Blood { get; protected set; }
        public override int Attack { get; protected set; }
        public override int HP { get; protected set; }
        public override int DefensivePower { get; protected set; }
        public override int Speed { get; protected set; }
        public override List<PileOfItem> FallenObjects { get; protected set; }

        public EliteMonster()
        {
            Blood = 200;
            HP = 200;
            Attack = 30;
            DefensivePower = 15;
            Speed = 1;
            //FallenObjects = new List<Item> { new Item("Legendary Sword", 1), new Item("Gold", 100) };
        }

        public override void TakeDamage(int damage)
        {
            Blood -= Math.Max(damage - DefensivePower / 2, 1); // Boss减伤逻辑
            if (Blood < 0) Blood = 0;
        }

        public override bool IsDead() => Blood <= 0;
    }
}
