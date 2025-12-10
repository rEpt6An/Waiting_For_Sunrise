
using Assets.C_.item;

namespace Assets.C_.common
{
    public class DescriptionGenerater
    {
        public static string GetEquipmentDescription(Equipment equipment)
        {
            return $"Max HP Increase: {equipment.MaxHP}\n" +
                   $"Damage Multiplier: {equipment.DamageMultiple:P0}\n" +
                   $"Max HP: {equipment.MaxHP}\n" +
                   $"Defense: {equipment.DefensivePower}\n" +
                   $"Dodge Rate: {equipment.Dodge:P2}\n" +
                   $"Attack Speed: {equipment.AttackSpeed:F2}\n" +
                   $"Hit Rate: {equipment.HitRate:P2}\n" +
                   $"Vision Range: {equipment.Scope}\n" +
                   $"Movement Speed: {equipment.Speed}\n" +
                   $"Critical Chance: {equipment.CriticalChance:P2}\n" +
                   $"Critical Damage Multiplier: {equipment.CriticalDamage:F1}x\n" +
                   $"Life Steal: {equipment.Leech:P2}\n" +
                   $"Self-Healing: {equipment.SelfHealing}\n" +
                   $"Luck: {equipment.Lucky}\n" +
                   $"Harvest Amount: {equipment.Harvest}\n" +
                   $"Melee Attack: {equipment.MeleeAttack}\n" +
                   $"Ranged Attack: {equipment.RangedAttack}";
        }
        
    }
}