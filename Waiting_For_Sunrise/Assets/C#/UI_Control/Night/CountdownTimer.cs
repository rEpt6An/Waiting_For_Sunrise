// 📜 CountdownTimer.cs (修正后)
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CountdownTimer : MonoBehaviour
{
    [Header("计时器设置")]
    [Tooltip("倒计时的总时长（秒）")]
    [SerializeField] private float timeDuration = 60f;

    private TextMeshProUGUI timerText;
    private float timer;

    [Header("UI & 管理引用")]
    [Tooltip("倒计时结束时弹出的 DayCompletionPanel")]
    [SerializeField] private DayCompletionPanel dayCompletionPanel;

    // 场景里如果没有挂 SceneSwitcher，就自动找一个 (保留)
    [Tooltip("负责切换场景的组件（必须在 Inspector 中引用或场景中存在）")]
    [SerializeField] private SceneSwitcher sceneSwitcher;


    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();

        // 场景里如果没有挂 SceneSwitcher，就自动找一个
        if (sceneSwitcher == null) sceneSwitcher = FindObjectOfType<SceneSwitcher>();

        // 自动查找 DayCompletionPanel
        if (dayCompletionPanel == null) dayCompletionPanel = FindObjectOfType<DayCompletionPanel>();
    }

    void Start() => ResetTimer();

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            timer = 0;
            UpdateTimerDisplay();
            this.enabled = false;

            // ⭐️ 核心修正：将结束逻辑交给 GameManager 处理
            if (GameManager.Instance != null)
            {
                GameManager.Instance.HandleDayEnd(dayCompletionPanel, sceneSwitcher);
            }
            else
            {
                Debug.LogError("无法找到 GameManager 实例，无法处理 Day End 逻辑！");
            }
        }
    }

    public void ResetTimer()
    {
        timer = timeDuration;
        UpdateTimerDisplay();
        this.enabled = true;
    }

    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(timer);
        timerText.text = $"{seconds}";
    }
}