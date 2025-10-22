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

        public int Blood { get; private set; } = 0;
        public double DamageMultiple { get; private set; } = 0;
        public int MaxHP { get; private set; } = 0;
        public int DefensivePower { get; private set; } = 0;
        public double Dodge { get; private set; } = 0;
        public double AttackSpeed { get; private set; } = 0;
        public double HitRate { get; private set; } = 0;
        public int Scope { get; private set; } = 0;
        public int Speed { get; private set; } = 0;
        public double CriticalChance { get; private set; } = 0;
        public double CriticalDamage { get; private set; } = 0;
        public double Leech { get; private set; } = 0;
        public int SelfHealing { get; private set; } = 0;
        public int Lucky { get; private set; } = 0;
        public int Harvest { get; private set; } = 0;
        public int MeleeAttack { get; private set; } = 0;
        public int RangedAttack { get; private set; } = 0;

        int IEquipment.Id => Id;

        public abstract void Execute(IPlayerState playState);

        public abstract void ExecuteRemove(IPlayerState playState);
    }
}