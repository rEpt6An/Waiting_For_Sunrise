// 📜 DayCompletionPanel.cs (修正后)
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DayCompletionPanel : MonoBehaviour
{
    [Header("UI 引用")]
    [Tooltip("显示当前完成天数的文本")]
    [SerializeField] private TextMeshProUGUI dayCompleteText;
    [Tooltip("显示下一波 Boss 信息的文本")]
    [SerializeField] private TextMeshProUGUI bossForecastText;
    [SerializeField] private Button shopButton;

    [Header("配置")]
    // 您的 Boss 天数配置
    private readonly int[] bossDays = { 5, 8, 10 };
    private const string SHOP_SCENE_NAME = "Shop"; // 商店场景名称

    private SceneSwitcher sceneSwitcher;

    void Start()
    {
        gameObject.SetActive(false);
        sceneSwitcher = FindObjectOfType<SceneSwitcher>();

        if (shopButton != null)
        {
            shopButton.onClick.RemoveAllListeners();
            shopButton.onClick.AddListener(OnShopButtonClicked);
        }
    }

    public void Show(int currentDay)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏

        // --- 1. 更新天数完成文本 ---
        if (dayCompleteText != null)
        {
            dayCompleteText.text = $"Day {currentDay} is completed";
        }

        // --- 2. 计算并显示下一波 Boss 预告 ---
        int nextBossDay = CalculateNextBossDay(currentDay);

        if (bossForecastText != null)
        {
            if (nextBossDay > 0)
            {
                bossForecastText.text = $"The next boss will arrive on Day {nextBossDay}.";
            }
            else
            {
                // 如果所有 Boss 都已过去（即 Day 10 结束，但尚未跳转到 VictoryPanel）
                bossForecastText.text = "All waves are completed! Prepare for the final challenge.";
            }
        }
    }

    /// <summary>
    /// 计算下一次 Boss 出现的天数。
    /// </summary>
    private int CalculateNextBossDay(int currentDay)
    {
        foreach (int bossDay in bossDays)
        {
            if (bossDay > currentDay)
            {
                return bossDay; // 找到第一个大于当前天数的 Boss 日
            }
        }
        return -1; // 没有更多的 Boss 日
    }

    /// <summary>
    /// 按钮点击事件：进入商店。
    /// </summary>
    private void OnShopButtonClicked()
    {
        Time.timeScale = 1f; // 恢复游戏时间
        gameObject.SetActive(false);

        // 切换场景
        if (sceneSwitcher != null)
        {
            sceneSwitcher.SwitchScene(SHOP_SCENE_NAME);
        }
        else
        {
            Debug.LogWarning("DayCompletionPanel: 找不到 SceneSwitcher，无法跳转 Shop！");
        }
    }
}