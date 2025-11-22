

using Assets.C_.bus;
using Assets.C_.player.player;

namespace Assets.C_.player
{
    public abstract class AbstractAttackable : IAttackable
    {
        public void GetAttack(Damage damage)
        {
            Damage actualDamage = DamageCalculator.PlayerGetDamage(new PlayerGetDamageContext(damage.Source, damage.DamageValue, GetDefensivePower(), GetDodge()));
            ChangeBlood(-actualDamage.DamageValue);
            // 击退

            AfterDamageHappened(damage);
        }

        protected void AfterDamageHappened(Damage damage)
        {
            if (damage.LifeSteal > 0)
            {
                // 发布吸血事件
                EventBus.Publish(new LifeStealEvent(GetSuckBlood(damage.DamageValue, damage.LifeSteal), damage.Source));
            }
        }


        protected int GetSuckBlood(int damageValue, double lifeSteal)
        {
            return (int)(damageValue * lifeSteal);
        }

        protected abstract int GetDefensivePower();
        protected abstract double GetDodge();
        protected abstract void ChangeBlood(int value);
    }
}