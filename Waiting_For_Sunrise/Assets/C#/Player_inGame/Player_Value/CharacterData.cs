using UnityEngine;

// 使用 [System.Serializable] 标记的类可以被Unity序列化，
// 这意味着它们可以显示在Inspector中，并且可以轻松转换为JSON。

public enum AttackType { Melee, Ranged }
public enum DamageScaleType { Melee, Ranged } // 近战 & 远程
// 用来决定武器伤害受哪个玩家属性加成

[System.Serializable]
public class PlayerStats
{
    [Header("核心属性")]
    public float maxHealth = 100f;   
    public float currentHealth = 100f;
    public float moveSpeed = 5f;

    [Header("攻击属性")]
    public float meleeAttack = 0;  // 近战伤害基数
    public float rangedAttack = 0; // 远程伤害基数
    [Tooltip("所有伤害的最终倍率, 1.0 = 100%")]
    public float damageMultiplier = 1.0f;
    [Tooltip("攻击速度倍率, 1.0 = 100%")]
    public float attackSpeedMultiplier = 1.0f;
    [Tooltip("攻击和技能范围, 也是拾取范围")]
    public float areaSize = 1.0f;

    [Header("防御属性")]
    [Tooltip("计算公式：x/x+10")]
    [Range(0f, 1f)]
    public float defense = 0f;
    [Tooltip("闪避几率, 0.1 = 10%. 上限由逻辑控制")]
    [Range(0f, 1f)]
    public float miss = 0f;

    [Header("命中率")]
    public float accuracy = 1.0f; // 命中率
    [Tooltip("暴击几率, 0.1 = 10%")]
    [Range(0f, 1f)]
    public float critChance = 0.05f;
    [Tooltip("暴击伤害倍率, 2.0 = 200%")]
    public float critMultiplier = 2.0f;
    [Tooltip("每次攻击回复1点生命的几率, 0.1 = 10%")]
    [Range(0f, 1f)]
    public float lifestealChance = 0f;
    [Tooltip("每5秒回复的生命值")]
    public float healthRegen = 0f;

    [Header("成长属性")]
    public float luck = 1.0f; // 幸运值, 影响概率事件
    [Tooltip("获取经验/金币的倍率, 1.0 = 100%")]
    public float harvestMultiplier = 1.0f;

    public float currentExperience = 0f;//现在经验
}

[System.Serializable]
public class WeaponStats
{
    [Header("基本信息")]
    public string weaponName = "武器";
    public AttackType attackType = AttackType.Melee;
    [Tooltip("发射的弹丸/攻击判定的数量")]
    public int projectileCount = 1;

    [Header("伤害计算")]
    public float baseDamage = 5f;
    [Tooltip("这个武器的伤害受玩家哪个属性加成")]
    public DamageScaleType damageScaleType = DamageScaleType.Melee;
    [Tooltip("受玩家属性加成的倍率")]
    public float scalingMultiplier = 0.5f;

    [Header("攻击节奏")]
    [Tooltip("武器的基础攻击速度 (每秒攻击次数)")]
    public float baseAttackSpeed = 1f;
    [Tooltip("远程武器的散射角度 / 近战武器的攻击弧度")]
    public float spreadAngle = 0f;
    [Tooltip("武器的基础攻击范围")]
    public float baseRange = 1.5f;
    public float knockback = 1f;

    [Header("特殊属性加成")]
    [Tooltip("武器提供的额外吸血几率")]
    public float lifestealBonus = 0f;
    [Tooltip("暴击率乘数, 1.1 = 增加10%当前暴击率")]
    public float critChanceMultiplier = 1.0f;
    [Tooltip("暴击伤害乘数, 1.1 = 增加10%当前暴击伤害")]
    public float critDamageMultiplier = 1.0f;

    [Header("远程武器专属")]
    public int clipSize = 10;
    [Tooltip("基础换弹时间 (秒)")]
    public float baseReloadTime = 2f;
}