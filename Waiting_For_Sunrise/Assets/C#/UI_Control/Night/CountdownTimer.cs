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

    [Header("倒计时结束跳转")]
    [Tooltip("负责切换场景的组件（若场景里已有可留空）")]
    [SerializeField] private SceneSwitcher sceneSwitcher;//所需引用

    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();

        // 场景里如果没有挂 SceneSwitcher，就自动找一个
        if (sceneSwitcher == null) sceneSwitcher = FindObjectOfType<SceneSwitcher>();
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

            if (sceneSwitcher != null)
                sceneSwitcher.SwitchScene("Shop");
            else
                UnityEngine.Debug.LogWarning("CountdownTimer: 找不到 SceneSwitcher，无法跳转 Shop！");
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