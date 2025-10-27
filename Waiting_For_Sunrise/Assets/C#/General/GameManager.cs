using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 单例模式
    public static GameManager Instance { get; private set; }

    // --- 游戏状态数据 ---
    public int Day { get; private set; } = 1; 
    private const int MAX_DAYS = 20; 

    void Awake()
    {
        // --- 单例实现 ---
        if (Instance != null && Instance != this)
        {
            // 如果场景中已存在一个GameManager，销毁这个新的
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // --- 跨场景存在 ---
        // 让这个GameManager对象在加载新场景时不被销毁
        DontDestroyOnLoad(gameObject);

        // --- 监听场景加载事件 ---
        // 这是在场景加载后执行逻辑的关键
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 当GameManager被销毁时，取消事件监听，防止内存泄漏
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 每当一个新场景加载完成时，这个方法就会被调用
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"场景 '{scene.name}' 已加载. 当前是第 {Day} 天.");

        // 核心逻辑：判断是否是从Night场景切换到了Shop场景
        // 我们需要知道上一个场景是什么，但这比较复杂。
        // 一个更简单可靠的方法是，只在进入Shop场景时增加天数。
        if (scene.name == "Shop")
        {
            // 进入商店，天数+1
            IncrementDay();
        }

        // 如果下一个场景是Night，我们需要检查是否胜利
        if (scene.name == "Night")
        {
            CheckForVictory();
        }
    }

    private void IncrementDay()
    {
        if (Day < MAX_DAYS)
        {
            Day++;
            Debug.Log($"新的一天开始了! 现在是第 {Day} 天.");
        }
        else
        {
            // 这通常不应该发生，因为在进入Night场景前就应该胜利了
            Debug.Log("已经达到最大天数，但仍在增加天数？");
        }
    }

    private void CheckForVictory()
    {
        // 当day达到20的时候 night 时间结束了是游戏胜利
        // 这意味着，当第20天的Night倒计时结束，进入Shop时，天数变为21。
        // 所以我们应该是在进入第21天的Night之前胜利，也就是第20天结束时。
        // 这里的逻辑是：如果当前天数已经达到20，并且我们即将开始Night场景，就胜利。
        if (Day >= MAX_DAYS)
        {
            // 游戏胜利的逻辑
            HandleGameVictory();
        }
        else
        {
            Debug.Log($"第 {Day} 天的夜晚开始...");
            // 如果有需要，可以在这里重置倒计时器等
            // FindObjectOfType<CountdownTimer>()?.ResetTimer();
        }
    }

    private void HandleGameVictory()
    {
        // TODO: 在这里实现游戏胜利的逻辑
        // 例如：
        // 1. 禁用玩家控制
        // 2. 显示胜利UI面板
        // 3. 加载胜利场景或主菜单
        Debug.LogWarning("游戏胜利！你成功存活了 " + MAX_DAYS + " 天！");

        // 示例：跳转到胜利场景
        // SceneManager.LoadScene("VictoryScene");
    }

    // 你可以添加一个方法来获取当前天数的字符串，方便UI显示
    public string GetDayString()
    {
        return $"Day: {Day}";
    }
}