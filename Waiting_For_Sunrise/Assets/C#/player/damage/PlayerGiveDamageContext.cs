using Assets.C_.common;

namespace Assets.C_.player
{
    public class PlayerGiveDamageContext
    {
        public WeaponType WeaponType { get; set; }
        public int WeaponBaseAttack {  get; set; }
        public double WeaponDamageMultiplier { get; set; }
        public double WeaponKnockback { get; private set; }
        public double WeaponLifeSteal { get; private set; }
        public double WeaponCriticalRate { get; private set; }
        public double WeaponCriticalMultiplier { get; private set; }

        public double PlayerDamageMultipler { get; private set; }
        public double PlayerHitRate { get; private set; }
        public double PlayerCriticalChance { get; private set; }
        public double PlayerCriticalDamage { get; private set; }
        public double PlayerLeech { get; private set; }
        public int PlayerHarvest { get; private set; }
        public int PlayerMeleeAttack { get; private set; }
        public int PlayerRangedAttack { get; private set; }

        public PlayerGiveDamageContext(int weaponBaseAttack, WeaponType weaponType = WeaponType.melee, double weaponDamageMultiplier = 0, double weaponKnockback = 0, double weaponLifeSteal = 0, double weaponCriticalRate = 0, double weaponCriticalMultiplier = 0, double playerDamageMultipler = 0, double playerHitRate = 0, double playerCriticalChance = 0, double playerCriticalDamage = 0, double playerLeech = 0, int playerHarvest = 0, int playerMeleeAttack = 0, int playerRangedAttack = 0)
        {
            WeaponType = weaponType;
            WeaponBaseAttack = weaponBaseAttack;
            WeaponDamageMultiplier = weaponDamageMultiplier;
            WeaponKnockback = weaponKnockback;
            WeaponLifeSteal = weaponLifeSteal;
            WeaponCriticalRate = weaponCriticalRate;
            WeaponCriticalMultiplier = weaponCriticalMultiplier;
            PlayerDamageMultipler = playerDamageMultipler;
            PlayerHitRate = playerHitRate;
            PlayerCriticalChance = playerCriticalChance;
            PlayerCriticalDamage = playerCriticalDamage;
            PlayerLeech = playerLeech;
            PlayerHarvest = playerHarvest;
            PlayerMeleeAttack = playerMeleeAttack;
            PlayerRangedAttack = playerRangedAttack;
        }
    }
}