
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.C_.player.player
{
    class PlayerState
    {
        //当前生命值
        public int blood { get; private set; }
        //经验值
        public int experience { get; private set; }
        //伤害倍率
        public int damageMultipler { get; private set; }
        //最大生命值
        public int HP { get; private set; }
        //防御力
        public int defensivePower { get; private set; }
        //闪避
        public int dodge { get; private set; }
        //命中率
        public int hitRate { get; private set; }
        //范围
        public int scope { get; private set; }
        //速度
        public int speed { get; private set; }
        //暴击率
        public int criticalChance { get; private set; }
        //暴击倍率（暴击伤害）
        public int criticalDamage { get; private set; }
        //吸血
        public int leech { get; private set; }
        //自愈
        public int selfHealing { get; private set; }
        //幸运
        public int lucky { get; private set; }
        //收获
        public int harvest { get; private set; }

      
        public PlayerState()
        {
            blood = 100;          // 当前生命值
            HP = 100;             // 最大生命值
            experience = 0;       // 经验值
            damageMultipler = 1;  // 伤害倍率（1 = 100%）
            defensivePower = 5;   // 防御力
            dodge = 10;           // 闪避率（%）
            hitRate = 80;         // 命中率（%）
            scope = 1;            // 范围
            speed = 5;            // 速度
            criticalChance = 5;   // 暴击率（%）
            criticalDamage = 150; // 暴击伤害（150 = 1.5倍）
            leech = 0;            // 吸血（%）
            selfHealing = 1;      // 自愈（每回合恢复生命值）
            lucky = 5;            // 幸运（影响掉落率）
            harvest = 0;          // 收获（额外资源获取）
        }
        public void changeBlood(int changePoint)
        {
            this.blood += changePoint;
            if (this.blood > this.HP)
            {
                this.blood = this.HP;
            }
        }
        public Boolean isDie()
        {
            if (blood <= 0)
            {
                return true;
            }
            return false;
        }
        public void changeExperience(int changePoint)
        {
            this.experience += changePoint;
        }
        public void changeDamageMultipler(int changePoint)
        {
            this.damageMultipler += changePoint;
        }
        public void changeHP(int changePoint)
        {
            this.HP += changePoint;
        }
        public void changeDefensivePower(int changePoint)
        {
            defensivePower += changePoint;
        }
        public void changeDodge(int changePoint)
        {
            dodge += changePoint;
            if (dodge >= 90)
            {
                dodge = 90;
            }
        }
        public void changeHitRate(int changePoint)
        {
            hitRate += changePoint;
        }

        public void changeScope(int changePoint)
        {
            scope += changePoint;
        }
        public void changeSpeed(int changePoint)
        {
            speed += changePoint;
        }
        public void changeCriticalChance(int changePoint)
        {
            criticalChance += changePoint;
        }
        public void changeCriticalDamage(int changePoint)
        {
            criticalDamage += changePoint;
        }
        public void changeLeech(int changePoint)
        {
            leech += changePoint;
        }
        public void changeSelfHealing(int changePoint)
        {
            selfHealing += changePoint;
        }
        public void changeLucky(int changePoint)
        {
            lucky += changePoint;
        }
        public void changeHarvest(int changePoint)
        {
            harvest += changePoint;
        }
    }
}