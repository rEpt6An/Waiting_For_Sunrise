namespace Assets.C_.player.player
{
    public interface IPlayerState
    {
        //  Ù–‘
        int Blood { get; }
        int Experience { get; }
        double DamageMultipler { get; }
        int MaxHP { get; }
        int DefensivePower { get; }
        double Dodge { get; }
        double AttackSpeed { get; }
        double HitRate { get; }
        int Scope { get; }
        int Speed { get; }
        double CriticalChance { get; }
        double CriticalDamage { get; }
        double Leech { get; }
        int SelfHealing { get; }
        int Lucky { get; }
        int Harvest { get; }
        int MeleeAttack { get; }
        int RangedAttack { get; }

        // ∑Ω∑®
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