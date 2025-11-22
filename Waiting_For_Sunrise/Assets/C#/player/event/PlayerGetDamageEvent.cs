using Assets.C_.item;

namespace Assets.C_.player
{
    public class PlayerGetDamageEvent
    {
        public Damage actualDamage;
        public Damage orignDamage;

        public PlayerGetDamageEvent(Damage actualDamage, Damage orignDamage)
        {
            this.actualDamage = actualDamage;
            this.orignDamage = orignDamage;
        }
    }
}