// EnemySpawner.cs (修正版)
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("生成设置")]
    [SerializeField] private List<EnemyData> spawnableEnemies;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private float spawnRadius = 20f;
    [SerializeField] private int maxEnemies = 50;

    private Transform playerTransform;
    private float spawnTimer;

    void Start()
    {
        var player = FindObjectOfType<PlayerCharacter>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            // 如果找不到玩家，打印错误并禁用此脚本
            UnityEngine.Debug.LogError("EnemySpawner: 场景中找不到PlayerCharacter，无法生成怪物！");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        spawnTimer += Time.deltaTime;

        // 【核心修改】：使用 UnityEngine.Random
        // 为了避免反复调用 FindGameObjectsWithTag (性能较差)，我们可以在这里添加一个计数器
        // 但为了简单，暂时保持原样
        if (spawnTimer >= spawnInterval && GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
        {
            SpawnRandomEnemy();
            spawnTimer = 0f;
        }
    }

    private void SpawnRandomEnemy()
    {
        if (spawnableEnemies.Count == 0)
        {
            UnityEngine.Debug.LogWarning("Spawner没有可生成的怪物！");
            return;
        }

        // --- 【核心修改】：明确使用 UnityEngine.Random ---
        // 1. 从列表中随机选择一种怪物类型
        int randomIndex = UnityEngine.Random.Range(0, spawnableEnemies.Count);
        EnemyData enemyToSpawn = spawnableEnemies[randomIndex];

        // 2. 确定生成位置 (在玩家周围的一个圆环上)
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        Vector3 spawnPosition = playerTransform.position + (Vector3)(randomDirection * spawnRadius);

        // 3. 实例化该怪物对应的预制体
        // 确保 enemyToSpawn.enemyPrefab 已经被正确设置
        if (enemyToSpawn.enemyPrefab == null)
        {
            UnityEngine.Debug.LogError($"怪物数据 '{enemyToSpawn.name}' 没有指定预制体 (Enemy Prefab)！");
            return;
        }
        GameObject enemyInstance = Instantiate(enemyToSpawn.enemyPrefab, spawnPosition, Quaternion.identity);

        enemyInstance.tag = "Enemy";

        // 4. 获取怪物控制器并注入数据
        EnemyController controller = enemyInstance.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.Initialize(enemyToSpawn);
        }
    }
}