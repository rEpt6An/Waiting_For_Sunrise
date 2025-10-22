namespace Assets.C_.item
{
    public class EquipmentConverter
    {
        public static Equipment Do2E(EquipmentDo equipmentDo)
        {
            Equipment equipment = new Equipment(
                equipmentDo.Id,
                equipmentDo.Blood,
                equipmentDo.DamageMultiple,
                equipmentDo.MaxHP,
                equipmentDo.DefensivePower,
                equipmentDo.Dodge,
                equipmentDo.AttackSpeed,
                equipmentDo.HitRate,
                equipmentDo.Scope,
                equipmentDo.Speed,
                equipmentDo.CriticalChance,
                equipmentDo.CriticalDamage,
                equipmentDo.Leech,
                equipmentDo.SelfHealing,
                equipmentDo.Lucky,
                equipmentDo.Harvest,
                equipmentDo.MeleeAttack,
                equipmentDo.RangedAttack
            );

            return equipment;
        }
    }
}