using UnityEngine;
using System.Collections; // ⭐️ 必须导入，用于协程
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("天数数据")]
    [Tooltip("按顺序将Day_01, Day_02...拖拽到这里")]
    [SerializeField] private List<DayData> dayProgression;

    [Header("生成范围设置")]
    [Tooltip("生成区域的最小坐标 (左下角)")]
    [SerializeField] private Vector2 minSpawnPosition = new Vector2(-10, -10);
    [Tooltip("生成区域的最大坐标 (右上角)")]
    [SerializeField] private Vector2 maxSpawnPosition = new Vector2(10, 10);

    [Header("预警设置")] // ⭐️ 新增：预警图标设置
    [Tooltip("怪物生成前的预警图标预制体")]
    [SerializeField] private GameObject warningIconPrefab;
    [Tooltip("预警图标显示的时长 (秒)")]
    [SerializeField] private float warningDuration = 0.5f;

    // --- 内部状态 ---
    private Transform playerTransform;
    private int currentDayIndex = -1;
    private int currentWaveIndex = -1;
    private DayData currentDayData;
    private WaveData currentWaveData;

    private float waveTimer;
    private float spawnTimer;
    private int enemiesSpawnedThisWave;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        var player = FindObjectOfType<PlayerCharacter>();
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            UnityEngine.Debug.LogError("EnemySpawner: 场景中找不到PlayerCharacter！");
            this.enabled = false;
            return;
        }

        StartNextDay();
    }
    // ---
    void Update()
    {
        if (playerTransform == null || currentWaveData == null) return;

        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= currentWaveData.spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemyGroup();
        }

        if (waveTimer >= currentWaveData.duration)
        {
            StartNextWave();
        }
    }

    private void StartNextDay()
    {
        int dayNumber = 1;
        if (GameManager.Instance != null)
        {
            dayNumber = GameManager.Instance.Day;
        }
        currentDayIndex = dayNumber - 1;

        if (currentDayIndex < 0 || currentDayIndex >= dayProgression.Count)
        {
            UnityEngine.Debug.LogError($"找不到第 {dayNumber} 天的数据！");
            this.enabled = false;
            return;
        }

        currentDayData = dayProgression[currentDayIndex];
        currentWaveIndex = -1;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentWaveIndex++;
        if (currentWaveIndex >= currentDayData.waves.Count)
        {
            UnityEngine.Debug.Log($"第 {currentDayIndex + 1} 天的所有波次已完成！");
            this.enabled = false;
            return;
        }

        currentWaveData = currentDayData.waves[currentWaveIndex];
        waveTimer = 0f;
        spawnTimer = 0f;
        enemiesSpawnedThisWave = 0;

        UnityEngine.Debug.Log($"开始第 {currentDayIndex + 1} 天, 第 {currentWaveIndex + 1} 波...");
    }

    private void SpawnEnemyGroup()
    {
        activeEnemies.RemoveAll(e => e == null);

        if (activeEnemies.Count >= currentDayData.maxEnemiesOnScreen) return;

        int totalEnemiesInWave = 0;
        foreach (var group in currentWaveData.enemyGroups) totalEnemiesInWave += group.count;
        if (enemiesSpawnedThisWave >= totalEnemiesInWave) return;

        EnemyGroup groupToSpawn = currentWaveData.enemyGroups[UnityEngine.Random.Range(0, currentWaveData.enemyGroups.Count)];
        enemiesSpawnedThisWave++;

        // ⭐️ 核心修改：启动协程来处理预警和生成
        StartCoroutine(SpawnEnemyWithWarning(groupToSpawn.enemyData));
    }

    /// <summary>
    /// 在生成怪物前显示预警图标的协程
    /// </summary>
    private IEnumerator SpawnEnemyWithWarning(EnemyData enemyToSpawn) // ⭐️ 替代原有的 SpawnSingleEnemy
    {
        // 1. 在指定的矩形区域内随机生成一个坐标
        float spawnX = UnityEngine.Random.Range(minSpawnPosition.x, maxSpawnPosition.x);
        float spawnY = UnityEngine.Random.Range(minSpawnPosition.y, maxSpawnPosition.y);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        GameObject warningInstance = null;

        // 2. 实例化预警图标 (如果预制体存在)
        if (warningIconPrefab != null)
        {
            // ⭐️ 修正：直接在 spawnPosition 实例化
            warningInstance = Instantiate(warningIconPrefab, spawnPosition, Quaternion.identity);

            // 确保 Z 轴在背景层级上，如果您的游戏需要，可以调整 Z 轴的值
            // warningInstance.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0); // 这一行现在是多余的，但如果需要调整Z轴可以保留

            UnityEngine.Debug.Log($"EnemySpawner: ⚠️ 在 {spawnPosition} 显示预警图标。");
        }

        // 3. 暂停预警时长 (0.5s)
        yield return new WaitForSeconds(warningDuration);

        // 4. 销毁预警图标 (核心销毁逻辑)
        if (warningInstance != null)
        {
            // ⭐️ 确保销毁
            Destroy(warningInstance);
        }

        // 5. 实例化怪物
        GameObject enemyInstance = Instantiate(enemyToSpawn.enemyPrefab, spawnPosition, Quaternion.identity);
        enemyInstance.tag = "Enemy";
        activeEnemies.Add(enemyInstance);

        PlayerCharacter playerChar = playerTransform.GetComponent<PlayerCharacter>();

        // 6. 初始化怪物
        EnemyController controller = enemyInstance.GetComponent<EnemyController>();
        if (controller != null)
        {
            controller.Initialize(enemyToSpawn, playerChar);
            UnityEngine.Debug.Log($"EnemySpawner: 初始化 {enemyInstance.name} 成功。");
        }
        else
        {
            UnityEngine.Debug.LogError($"EnemySpawner: ❌ 无法在 {enemyInstance.name} 上找到 EnemyController 或继承类！");
        }
    }

    // ---
    //显示出怪范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // 设置Gizmo的颜色
        Vector3 center = new Vector3(
            (minSpawnPosition.x + maxSpawnPosition.x) / 2,
            (minSpawnPosition.y + maxSpawnPosition.y) / 2,
            0
        );
        Vector3 size = new Vector3(
            maxSpawnPosition.x - minSpawnPosition.x,
            maxSpawnPosition.y - minSpawnPosition.y,
            0
        );
        Gizmos.DrawWireCube(center, size); // 绘制一个线框矩形
    }
}