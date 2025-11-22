using Assets.C_.common;
using Assets.C_.item;
using System;

namespace Assets.C_.player
{
    public static class DamageCalculator
    {
        private static readonly Random random = new Random();

        public static Damage PlayerGetDamage(PlayerGetDamageContext playerGetDamageContext)
        {
            double randomDouble = random.NextDouble();
            if (randomDouble > playerGetDamageContext.dodge) return new Damage(playerGetDamageContext.Source, 0);
            int actualDamage = playerGetDamageContext.value - playerGetDamageContext.defensivePower;
            if (actualDamage < 1) return new Damage(playerGetDamageContext.Source, 1);
            return new Damage(playerGetDamageContext.Source, actualDamage);
        }


        public static Damage PlayerGiveDamage(PlayerGiveDamageContext playerGiveDamageContext)
        {
            // TODO 
            return null;
        }
    }
}