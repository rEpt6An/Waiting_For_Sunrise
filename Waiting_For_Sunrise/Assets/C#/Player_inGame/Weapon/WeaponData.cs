using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    // 不在这里定义任何 enum

    [Header("核心数值")]
    public string weaponName = "测试武器";

    [Tooltip("攻击类型: 0=近战, 1=远程")]
    public int attackType = 0;

    [Tooltip("伤害加成类型: 0=受近战加成, 1=受远程加成")]
    public int damageScaleType = 0;

    public float baseDamage = 10f;
    public float scalingMultiplier = 5f;
    public float baseAttackSpeed = 1f;

    [Tooltip("武器的基础攻击范围/尺寸")]
    public float baseRange = 1.5f;
}