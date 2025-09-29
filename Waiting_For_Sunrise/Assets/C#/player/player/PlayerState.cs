
﻿using System;

namespace Assets.C_.player.player
{
    class PlayerState: IPlayerState
    {
        //当前生命值
        public int Blood { get; private set; }
        //经验值
        public int Experience { get; private set; }
        //伤害倍率
        public int DamageMultipler { get; private set; }
        //最大生命值
        public int HP { get; private set; }
        //防御力
        public int DefensivePower { get; private set; }
        //闪避
        public int Dodge { get; private set; }
        //命中率
        public int HitRate { get; private set; }
        //范围
        public int Scope { get; private set; }
        //速度
        public int Speed { get; private set; }
        //暴击率
        public int CriticalChance { get; private set; }
        //暴击倍率（暴击伤害）
        public int CriticalDamage { get; private set; }
        //吸血
        public int Leech { get; private set; }
        //自愈
        public int SelfHealing { get; private set; }
        //幸运
        public int Lucky { get; private set; }
        //收获
        public int Harvest { get; private set; }

      
        public PlayerState()
        {
            Blood = 100;          // 当前生命值
            HP = 100;             // 最大生命值
            Experience = 0;       // 经验值
            DamageMultipler = 1;  // 伤害倍率（1 = 100%）
            DefensivePower = 5;   // 防御力
            Dodge = 10;           // 闪避率（%）
            HitRate = 80;         // 命中率（%）
            Scope = 1;            // 范围
            Speed = 5;            // 速度
            CriticalChance = 5;   // 暴击率（%）
            CriticalDamage = 150; // 暴击伤害（150 = 1.5倍）
            Leech = 0;            // 吸血（%）
            SelfHealing = 1;      // 自愈（每回合恢复生命值）
            Lucky = 5;            // 幸运（影响掉落率）
            Harvest = 0;          // 收获（额外资源获取）
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
        public void changeExperience(int changePoint)
        {
            this.Experience += changePoint;
        }
        public void changeDamageMultipler(int changePoint)
        {
            this.DamageMultipler += changePoint;
        }
        public void changeHP(int changePoint)
        {
            this.HP += changePoint;
        }
        public void changeDefensivePower(int changePoint)
        {
            DefensivePower += changePoint;
        }
        public void changeDodge(int changePoint)
        {
            Dodge += changePoint;
            if (Dodge >= 90)
            {
                Dodge = 90;
            }
        }
        public void changeHitRate(int changePoint)
        {
            HitRate += changePoint;
        }

        public void changeScope(int changePoint)
        {
            Scope += changePoint;
        }
        public void changeSpeed(int changePoint)
        {
            Speed += changePoint;
        }
        public void changeCriticalChance(int changePoint)
        {
            CriticalChance += changePoint;
        }
        public void changeCriticalDamage(int changePoint)
        {
            CriticalDamage += changePoint;
        }
        public void changeLeech(int changePoint)
        {
            Leech += changePoint;
        }
        public void changeSelfHealing(int changePoint)
        {
            SelfHealing += changePoint;
        }
        public void changeLucky(int changePoint)
        {
            Lucky += changePoint;
        }
        public void changeHarvest(int changePoint)
        {
            Harvest += changePoint;
        }
    }
}