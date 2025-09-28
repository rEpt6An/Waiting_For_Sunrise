using Assets.C_.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.C_.monster
{
    public class NormalMonster : MonsterBase
    {
        public override int Blood { get; protected set; }
        public override int Attack { get; protected set; }
        public override int HP { get; protected set; }
        public override int DefensivePower { get; protected set; }
        public override int Speed { get; protected set; }
        public override List<Item> FallenObjects { get; protected set; }
        

        public NormalMonster()
        {
            Blood = 50;
            HP = 50;
            Attack = 10;
            DefensivePower = 5;
            Speed = 3;
            //FallenObjects = new List<Item> { new Item("Gold", 10) };
        }

        public override void TakeDamage(int damage)
        {
            Blood -= Math.Max(damage - DefensivePower, 1);
            if (Blood < 0) Blood = 0;
        }

        public override bool IsDead() => Blood <= 0;
    }
}
