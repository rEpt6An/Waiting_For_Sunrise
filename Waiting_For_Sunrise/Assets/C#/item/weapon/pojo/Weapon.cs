using Assets.C_.common;

namespace Assets.C_.item
{
    public class Weapon
    {
        public int Id { get; private set; }

        /// <summary>
        /// 武器类型
        /// </summary>
        public WeaponType WeaponType { get; private set; }

        /// <summary>
        /// 效果衍生次数
        /// </summary>
        public int EffectDerivationCount { get; private set; }

        /// <summary>
        /// 基础攻击
        /// </summary>
        public int BaseAttack { get; private set; }

        /// <summary>
        /// 对应伤害倍率
        /// </summary>
        public double DamageMultiplier { get; private set; }

        /// <summary>
        /// 攻速
        /// </summary>
        public double AttackSpeed { get; private set; }

        /// <summary>
        /// 弧度
        /// </summary>
        public double Arc { get; private set; }

        /// <summary>
        /// 范围
        /// </summary>
        public double Range { get; private set; }

        /// <summary>
        /// 击退
        /// </summary>
        public double Knockback { get; private set; }

        /// <summary>
        /// 吸血
        /// </summary>
        public double LifeSteal { get; private set; }

        /// <summary>
        /// 暴击率
        /// </summary>
        public double CriticalRate { get; private set; }

        /// <summary>
        /// 暴击倍率
        /// </summary>
        public double CriticalMultiplier { get; private set; }

        /// <summary>
        /// 弹夹
        /// </summary>
        public int Magazine { get; private set; }

        /// <summary>
        /// 换弹速度/s
        /// </summary>
        public double ReloadSpeed { get; private set; }

        public Weapon(int id, string weaponType, int effectDerivationCount, int baseAttack,
                     double damageMultiplier, double attackSpeed, double arc, double range,
                     double knockback, double lifeSteal, double criticalRate,
                     double criticalMultiplier, int magazine, double reloadSpeed)
        {
            Id = id;
            WeaponType = weaponType;
            EffectDerivationCount = effectDerivationCount;
            BaseAttack = baseAttack;
            DamageMultiplier = damageMultiplier;
            AttackSpeed = attackSpeed;
            Arc = arc;
            Range = range;
            Knockback = knockback;
            LifeSteal = lifeSteal;
            CriticalRate = criticalRate;
            CriticalMultiplier = criticalMultiplier;
            Magazine = magazine;
            ReloadSpeed = reloadSpeed;
        }
    }
}