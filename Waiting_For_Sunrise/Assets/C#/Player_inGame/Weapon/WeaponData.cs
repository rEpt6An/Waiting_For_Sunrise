using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    // 不在这里定义任何 enum

    [Header("攻击体配置")]
    [Tooltip("攻击碰撞体/弹药的预制件。近战是碰撞区域，远程是子弹。")]
    public GameObject attackPrefab;
    [Tooltip("近战攻击碰撞体存在时间 (秒)")]
    public float meleeLifetime = 0.1f;

    [Tooltip("攻击体在 Instantiate 时的速度 (仅用于远程武器)")]
    public float projectileSpeed = 10f;

    [Header("核心数值")]
    public Sprite image;
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

    [Tooltip("击退")]
    public float repel = 1f;

    [Tooltip("弹夹")]
    public int clip = 1;

    public float cliptime = 1f;

    [Tooltip("穿透")]
    public int penetrate = 1;   //穿透

    [Header("动画配置")]

    [Tooltip("动画持续时间（往返）")]
    public float animationDuration = 0.2f;

    [Tooltip("旋转角度 (例如: 90)")]
    public float rotationAngle = 90f;



    [Tooltip("攻击时的声音效果 (可选)")]
    public AudioClip attackSound;
}