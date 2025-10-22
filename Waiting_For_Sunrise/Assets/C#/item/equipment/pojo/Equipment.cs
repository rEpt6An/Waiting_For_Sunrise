
using Assets.C_.player.player;
using Unity.VisualScripting;

namespace Assets.C_.item
{
    public class Equipment : AbstractEquipment
    {
        public Equipment(
            int id,
            int blood = 0,
            double damageMultiple = 0,
            int maxHP = 0,
            int defensivePower = 0,
            double dodge = 0,
            double attackSpeed = 0,
            double hitRate = 0,
            int scope = 0,
            int speed = 0,
            double criticalChance = 0,
            double criticalDamage = 0,
            double leech = 0,
            int selfHealing = 0,
            int lucky = 0,
            int harvest = 0,
            int meleeAttack = 0,
            int rangedAttack = 0
        ) : base(
            id, blood, damageMultiple, maxHP, defensivePower,
            dodge, attackSpeed, hitRate, scope, speed,
            criticalChance, criticalDamage, leech, selfHealing,
            lucky, harvest, meleeAttack, rangedAttack
        ){}

        public override void Execute(IPlayerState playState)
        {
            ApplyPlayerStateChangesOnWear(playState);
        }

        public override void ExecuteRemove(IPlayerState playState)
        {
            ApplyPlayerStateChangesOnUnWear(playState);
        }

        private void ApplyPlayerStateChangesOnWear(IPlayerState playerState)
        {
            if (Blood != 0)
                playerState.changeBlood(Blood);
            if (DamageMultiple != 0)
                playerState.changeDamageMultipler(DamageMultiple);
            if (MaxHP != 0)
                playerState.changeMaxHP(MaxHP);
            if (DefensivePower != 0)
                playerState.changeDefensivePower(DefensivePower);
            if (Dodge != 0)
                playerState.changeDodge(Dodge);
            if (AttackSpeed != 0)
                playerState.changeAttackSpeed(AttackSpeed);
            if (HitRate != 0)
                playerState.changeHitRate(HitRate);
            if (Scope != 0)
                playerState.changeScope(Scope);
            if (Speed != 0)
                playerState.changeSpeed(Speed);
            if (CriticalChance != 0)
                playerState.changeCriticalChance(CriticalChance);
            if (CriticalDamage != 0)
                playerState.changeCriticalDamage(CriticalDamage);
            if (Leech != 0)
                playerState.changeLeech(Leech);
            if (SelfHealing != 0)
                playerState.changeSelfHealing(SelfHealing);
            if (Lucky != 0)
                playerState.changeLucky(Lucky);
            if (Harvest != 0)
                playerState.changeHarvest(Harvest);
            if (MeleeAttack != 0)
                playerState.changeMeleeAttack(MeleeAttack);
            if (RangedAttack != 0)
                playerState.changeRangedAttack(RangedAttack);
        }

        private void ApplyPlayerStateChangesOnUnWear(IPlayerState playerState)
        {
            if (Blood != 0)
                playerState.changeBlood(-Blood);
            if (DamageMultiple != 0)
                playerState.changeDamageMultipler(-DamageMultiple);
            if (MaxHP != 0)
                playerState.changeMaxHP(-MaxHP);
            if (DefensivePower != 0)
                playerState.changeDefensivePower(-DefensivePower);
            if (Dodge != 0)
                playerState.changeDodge(-Dodge);
            if (AttackSpeed != 0)
                playerState.changeAttackSpeed(-AttackSpeed);
            if (HitRate != 0)
                playerState.changeHitRate(-HitRate);
            if (Scope != 0)
                playerState.changeScope(-Scope);
            if (Speed != 0)
                playerState.changeSpeed(-Speed);
            if (CriticalChance != 0)
                playerState.changeCriticalChance(-CriticalChance);
            if (CriticalDamage != 0)
                playerState.changeCriticalDamage(-CriticalDamage);
            if (Leech != 0)
                playerState.changeLeech(-Leech);
            if (SelfHealing != 0)
                playerState.changeSelfHealing(-SelfHealing);
            if (Lucky != 0)
                playerState.changeLucky(-Lucky);
            if (Harvest != 0)
                playerState.changeHarvest(-Harvest);
            if (MeleeAttack != 0)
                playerState.changeMeleeAttack(-MeleeAttack);
            if (RangedAttack != 0)
                playerState.changeRangedAttack(-RangedAttack);
        }
    }
}