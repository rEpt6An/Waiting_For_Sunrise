// DayData.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Day", menuName = "Waves/Day Data")]
public class DayData : ScriptableObject
{
    [Tooltip("这一天的所有怪物波次，会按顺序执行")]
    public List<WaveData> waves;

    [Header("全局设置")]
    [Tooltip("这一天场景中允许存在的最大怪物数量")]
    public int maxEnemiesOnScreen = 100;
}