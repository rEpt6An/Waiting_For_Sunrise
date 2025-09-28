using Assets.C_.common;
using Assets.C_.player.bag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.C_.monster
{
    class MonsterState

    {
        //当前生命值
        public int Blood { get; private set; }
        //攻击力
        public int Attack { get; private set; }
        //最大生命值
        public int HP { get; private set; }
        //防御力
        public int DefensivePower { get; private set; }
        //速度
        public int Speed { get; private set; }
        //掉落物
        public List<PileOfItem> FallenObjects { get; private set; }
        public MonsterState()
        {

        }
        public void changeBlood(int changePoint)
        {
            this.Blood += changePoint;
            if (this.Blood > this.HP)
            {
                this.Blood = this.HP;
            }
        }
        public Boolean isDie()
        {
            if (Blood <= 0)
            {
                return true;
            }
            return false;
        }
       
    }
