using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间
using Assets.C_.player.player;
using Assets.C_.player;

public class ExperienceBarUI : MonoBehaviour
{
    [Header("组件引用")]
    [SerializeField] private UnityEngine.UI.Slider experienceSlider;
    [SerializeField] private TextMeshProUGUI experienceText;

    [Header("升级设置")]
    [Tooltip("升级到下一级所需的总经验值")]
    public int experienceToLevelUp = 100; // 简化值

    private PlayerState _playerState;

    void Start()
    {
        _playerState = (PlayerState)Player.GetInstance().PlayerState;
        if (experienceSlider == null)
        {
            experienceSlider = GetComponentInChildren<UnityEngine.UI.Slider>();
        }
        if (experienceText == null)
        {
            experienceText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (_playerState != null)
        {
            UpdateDisplay();
        }
        else
        {
            UnityEngine.Debug.LogError("ExperienceBarUI: 无法找到 PlayerState.Instance！");
        }
    }

    void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_playerState != null && experienceSlider != null && experienceText != null)
        {
            // 更新Slider
            experienceSlider.maxValue = experienceToLevelUp;
            experienceSlider.value = _playerState.Experience;

            // 更新文本
            experienceText.text = $"{_playerState.Experience} / {experienceToLevelUp}";
        }
    }
}