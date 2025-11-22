namespace Assets.C_.player
{
    public class Damage
    {
        public string Source { get; set; }
        public int DamageValue { get; set; }
        public double LifeSteal { get; set; }
        public double Knockback { get; set; }
        public double Harvest { get; set; }

        public Damage(string source, int damageValue, double lifeSteal = 0, double knockback = 0, double harvest = 0)
        {
            Source = source;
            DamageValue = damageValue;
            LifeSteal = lifeSteal;
            Knockback = knockback;
            Harvest = harvest;
        }
    }
}