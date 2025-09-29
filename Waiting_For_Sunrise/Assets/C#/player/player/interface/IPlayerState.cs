namespace Assets.C_.player.player
{
    public interface IPlayerState
    {
        //  Ù–‘
        int Blood { get; }
        int Experience { get; }
        int DamageMultipler { get; }
        int HP { get; }
        int DefensivePower { get; }
        int Dodge { get; }
        int HitRate { get; }
        int Scope { get; }
        int Speed { get; }
        int CriticalChance { get; }
        int CriticalDamage { get; }
        int Leech { get; }
        int SelfHealing { get; }
        int Lucky { get; }
        int Harvest { get; }

        // ∑Ω∑®
        void changeBlood(int changePoint);
        bool isDie();
        void changeExperience(int changePoint);
        void changeDamageMultipler(int changePoint);
        void changeHP(int changePoint);
        void changeDefensivePower(int changePoint);
        void changeDodge(int changePoint);
        void changeHitRate(int changePoint);
        void changeScope(int changePoint);
        void changeSpeed(int changePoint);
        void changeCriticalChance(int changePoint);
        void changeCriticalDamage(int changePoint);
        void changeLeech(int changePoint);
        void changeSelfHealing(int changePoint);
        void changeLucky(int changePoint);
        void changeHarvest(int changePoint);
    }
}