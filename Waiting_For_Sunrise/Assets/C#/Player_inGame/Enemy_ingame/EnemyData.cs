// EnemyData.cs (更新版)
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("基本信息")]
    [Tooltip("怪物的名称")]
    public string enemyName = "新怪物";

    [Tooltip("这个怪物类型所使用的预-制体（包含动画和模型）")]
    public GameObject enemyPrefab;

    [Header("战斗属性")]
    public float maxHealth = 10f;

    public int attackDamage = 10; // 怪物的攻击力

    [Tooltip("怪物攻击一次后的冷却时间（秒）")]
    public float attackCooldown = 1f;

    [Header("移动与成长")]
    public float moveSpeed = 0.8f;
    public int experienceReward = 5;
    public int coinDropAmount = 1; // 怪物死亡时掉落的金币数量
}