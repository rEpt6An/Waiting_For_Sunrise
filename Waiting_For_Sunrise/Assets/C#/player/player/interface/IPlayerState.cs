namespace Assets.C_.player.player
{
    public interface IPlayerState
    {
        // 属性
        /// <summary>
        /// 当前生命值
        /// </summary>
        public int Blood { get; }

        /// <summary>
        /// 经验值
        /// </summary>
        public int Experience { get; }

        /// <summary>
        /// 伤害倍率
        /// </summary>
        public double DamageMultipler { get; }

        /// <summary>
        /// 最大生命值
        /// </summary>
        public int MaxHP { get; }

        /// <summary>
        /// 防御力
        /// </summary>
        public int DefensivePower { get; }

        /// <summary>
        /// 闪避率
        /// </summary>
        public double Dodge { get; }

        /// <summary>
        /// 攻击速度
        /// </summary>
        public double AttackSpeed { get; }

        /// <summary>
        /// 命中率
        /// </summary>
        public double HitRate { get; }

        /// <summary>
        /// 视野范围
        /// </summary>
        public int Scope { get; }

        /// <summary>
        /// 移动速度
        /// </summary>
        public int Speed { get; }

        /// <summary>
        /// 暴击几率
        /// </summary>
        public double CriticalChance { get; }

        /// <summary>
        /// 暴击伤害倍率
        /// </summary>
        public double CriticalDamage { get; }

        /// <summary>
        /// 吸血比例
        /// </summary>
        public double Leech { get; }

        /// <summary>
        /// 自我治疗量
        /// </summary>
        public int SelfHealing { get; }

        /// <summary>
        /// 幸运值
        /// </summary>
        public int Lucky { get; }

        /// <summary>
        /// 收获量
        /// </summary>
        public int Harvest { get; }

        /// <summary>
        /// 近战攻击力
        /// </summary>
        public int MeleeAttack { get; }

        /// <summary>
        /// 远程攻击力
        /// </summary>
        public int RangedAttack { get; }

        // 方法
        void changeBlood(int changePoint);
        bool isDie();
        void changeExperience(int changePoint);
        void changeDamageMultipler(double changePoint);
        void changeMaxHP(int changePoint);
        void changeAttackSpeed(double changePoint);
        void changeDefensivePower(int changePoint);
        void changeDodge(double changePoint);
        void changeHitRate(double changePoint);
        void changeScope(int changePoint);
        void changeSpeed(int changePoint);
        void changeCriticalChance(double changePoint);
        void changeCriticalDamage(double changePoint);
        void changeLeech(double changePoint);
        void changeSelfHealing(int changePoint);
        void changeLucky(int changePoint);
        void changeHarvest(int changePoint);
        void changeMeleeAttack(int changePoint);
        void changeRangedAttack(int changePoint);

    }
}