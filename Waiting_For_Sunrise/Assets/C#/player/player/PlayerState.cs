
﻿using System;

namespace Assets.C_.player.player
{
    public class PlayerState: IPlayerState
    {

        //当前生命值
        public int Blood { get; private set; }
        //经验值
        public int Experience { get; private set; }
        //伤害倍率
        public double DamageMultipler { get; private set; }
        //最大生命值
        public int MaxHP { get; private set; }
        //防御力
        public int DefensivePower { get; private set; }
        //闪避率
        public double Dodge { get; private set; }
        //命中率
        public double HitRate { get; private set; }
        //范围
        public int Scope { get; private set; }
        //速度
        public int Speed { get; private set; }
        //暴击率
        public double CriticalChance { get; private set; }
        //暴击倍率（暴击伤害的倍率）
        public double CriticalDamage { get; private set; }
        //吸血率
        public double Leech { get; private set; }
        //自愈
        public int SelfHealing { get; private set; }
        //幸运
        public int Lucky { get; private set; }
        //收获
        public int Harvest { get; private set; }
        // 攻速
        public double AttackSpeed { get; private set; }
        // 近战攻击力加成（值）
        public int MeleeAttack { get; private set; }
        // 远程攻击力加成（值）
        public int RangedAttack { get; private set; }

        public PlayerState()
        {
            Blood = 100;          // 当前生命值
            MaxHP = 100;             // 最大生命值
            Experience = 0;       // 经验值
            DamageMultipler = 1;  // 伤害倍率（1 = 100%）
            DefensivePower = 5;   // 防御力
            Dodge = 0.1;           // 闪避率（%）
            HitRate = 0.8;         // 命中率（%）
            Scope = 1;            // 范围
            Speed = 5;            // 速度
            CriticalChance = 0.05;// 暴击率（%）
            CriticalDamage = 1.5; // 暴击伤害（150 = 1.5倍）
            Leech = 0;            // 吸血（%）
            SelfHealing = 1;      // 自愈（每回合恢复生命值）
            Lucky = 5;            // 幸运（影响掉落率）
            Harvest = 0;          // 收获（额外资源获取）
            MeleeAttack = 0;
            RangedAttack = 0;
            AttackSpeed = 0.01; 
        }
        public void changeBlood(int changePoint)
        {
            this.Blood += changePoint;
            if (this.Blood > this.MaxHP) this.Blood = this.MaxHP;
            else if (this.Blood < 0) this.Blood = 0;
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
        public void changeDamageMultipler(double changePoint)
        {
            this.DamageMultipler += changePoint;
            DamageMultipler = FixPrecision(DamageMultipler);
        }
        public void changeMaxHP(int changePoint)
        {
            this.MaxHP += changePoint;
        }
        public void changeDefensivePower(int changePoint)
        {
            DefensivePower += changePoint;
        }
        public void changeDodge(double changePoint)
        {
            Dodge += changePoint;
            Dodge = FixPrecision(Dodge);
        }
        public void changeHitRate(double changePoint)
        {
            HitRate += changePoint;
            HitRate = FixPrecision(HitRate);
        }

        public void changeScope(int changePoint)
        {
            Scope += changePoint;
        }
        public void changeSpeed(int changePoint)
        {
            Speed += changePoint;
        }
        public void changeCriticalChance(double changePoint)
        {
            CriticalChance += changePoint;
            CriticalChance = FixPrecision(CriticalChance);
        }
        public void changeCriticalDamage(double changePoint)
        {
            CriticalDamage += changePoint;
            CriticalDamage = FixPrecision(CriticalDamage);
        }
        public void changeLeech(double changePoint)
        {
            Leech += changePoint;
            Leech = FixPrecision(Leech);
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

        public void changeAttackSpeed(double changePoint)
        {
            AttackSpeed += changePoint;
            AttackSpeed = FixPrecision(AttackSpeed);
        }

        public static double FixPrecision(double value, double epsilon = 1e-5)
        {
            // 如果值的绝对值小于 epsilon，则视为 0
            return Math.Abs(value) < epsilon ? 0 : value;
        }

        public void changeMeleeAttack(int changePoint)
        {
            MeleeAttack += changePoint;
        }

        public void changeRangedAttack(int changePoint)
        {
            RangedAttack += changePoint;
        }
    }
}