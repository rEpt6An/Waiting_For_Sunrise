// WaveData.cs
using UnityEngine;
using System.Collections.Generic;

// 这个 [System.Serializable] 很重要，它让我们可以
// 在另一个ScriptableObject的Inspector里编辑这个类的实例
[System.Serializable]
public class EnemyGroup
{
    public EnemyData enemyData; // 要生成的怪物类型
    public int count;           // 生成数量
}

[CreateAssetMenu(fileName = "New Wave", menuName = "Waves/Wave Data")]
public class WaveData : ScriptableObject
{
    public List<EnemyGroup> enemyGroups; // 这一波由哪些怪物组构成
    public float duration;               // 这一波持续多长时间（秒）
    public float spawnInterval;          // 在这一波期间，生成小怪群的间隔
}