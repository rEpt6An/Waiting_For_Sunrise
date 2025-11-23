using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间
using Assets.C_.player.player;
using Assets.C_.player;
public class HealthBarUI : MonoBehaviour
{
    [Header("组件引用")]
    [Tooltip("血条的Slider组件")]
    [SerializeField] private UnityEngine.UI.Slider healthSlider;

    [Tooltip("显示血量数值的TextMeshPro文本组件")]
    [SerializeField] private TextMeshProUGUI healthText;

    private PlayerState _playerState;

    void Start()
    {
        _playerState = (PlayerState)Player.GetInstance().PlayerState;
        // 如果没有在Inspector中手动拖拽引用，尝试自动查找
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<UnityEngine.UI.Slider>();
        }
        if (healthText == null)
        {
            healthText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (_playerState != null)
        {
            // 初始化UI
            UpdateDisplay();
        }
        else
        {
            UnityEngine.Debug.LogError("HealthBarUI: 无法找到 PlayerState.Instance！");
        }
    }

    void Update()
    {
        // 实时刷新UI
        UpdateDisplay();
    }

    /// <summary>
    /// 统一更新血条Slider和文本的方法
    /// </summary>
    private void UpdateDisplay()
    {
        if (_playerState != null && healthSlider != null && healthText != null)
        {
            // 更新Slider
            healthSlider.maxValue = _playerState.MaxHP;
            healthSlider.value = _playerState.Blood;

            // 更新文本
            // 使用 $"" 格式化字符串，清晰直观
            healthText.text = $"{_playerState.Blood} / {_playerState.MaxHP}";
        }
    }
}