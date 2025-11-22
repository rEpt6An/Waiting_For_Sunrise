using Assets.C_.player.player;

namespace Assets.C_.item
{
    public abstract class AbstractEquipment : IEquipment
    {
        public AbstractEquipment(int id, int blood = 0, double damageMultiple = 0, int maxHP = 0,
                int defensivePower = 0, double dodge = 0, double attackSpeed = 0,
                double hitRate = 0, int scope = 0, int speed = 0,
                double criticalChance = 0, double criticalDamage = 0,
                double leech = 0, int selfHealing = 0, int lucky = 0,
                int harvest = 0, int meleeAttack = 0, int rangedAttack = 0)
        {
            Id = id;
            Blood = blood;
            DamageMultiple = damageMultiple;
            MaxHP = maxHP;
            DefensivePower = defensivePower;
            Dodge = dodge;
            AttackSpeed = attackSpeed;
            HitRate = hitRate;
            Scope = scope;
            Speed = speed;
            CriticalChance = criticalChance;
            CriticalDamage = criticalDamage;
            Leech = leech;
            SelfHealing = selfHealing;
            Lucky = lucky;
            Harvest = harvest;
            MeleeAttack = meleeAttack;
            RangedAttack = rangedAttack;
        }

        public int Id { get; set; }

        /// <summary>
        /// 当前生命值
        /// </summary>
        public int Blood { get; private set; } = 0;

        /// <summary>
        /// 伤害倍率
        /// </summary>
        public double DamageMultiple { get; private set; } = 0;

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxHP { get; private set; } = 0;

        /// <summary>
        /// 防御力
        /// </summary>
        public int DefensivePower { get; private set; } = 0;

        /// <summary>
        /// 闪避率
        /// </summary>
        public double Dodge { get; private set; } = 0;

        /// <summary>
        /// 攻击速度
        /// </summary>
        public double AttackSpeed { get; private set; } = 0;

        /// <summary>
        /// 命中率
        /// </summary>
        public double HitRate { get; private set; } = 0;

        /// <summary>
        /// 视野范围
        /// </summary>
        public int Scope { get; private set; } = 0;

        /// <summary>
        /// 移动速度
        /// </summary>
        public int Speed { get; private set; } = 0;

        /// <summary>
        /// 暴击几率
        /// </summary>
        public double CriticalChance { get; private set; } = 0;

        /// <summary>
        /// 暴击伤害倍率
        /// </summary>
        public double CriticalDamage { get; private set; } = 0;

        /// <summary>
        /// 吸血比例
        /// </summary>
        public double Leech { get; private set; } = 0;

        /// <summary>
        /// 自我治疗量
        /// </summary>
        public int SelfHealing { get; private set; } = 0;

        /// <summary>
        /// 幸运值
        /// </summary>
        public int Lucky { get; private set; } = 0;

        /// <summary>
        /// 收获量
        /// </summary>
        public int Harvest { get; private set; } = 0;

        /// <summary>
        /// 近战攻击力
        /// </summary>
        public int MeleeAttack { get; private set; } = 0;

        /// <summary>
        /// 远程攻击力
        /// </summary>
        public int RangedAttack { get; private set; } = 0;

        int IEquipment.Id => Id;

        public abstract void Execute(IPlayerState playState);

        public abstract void ExecuteRemove(IPlayerState playState);
    }
}