using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间
using Assets.C_.player.player;
using Assets.C_.player;

public class ExperienceBarUI : MonoBehaviour
{
    [Header("组件引用")]
    [SerializeField] private UnityEngine.UI.Slider experienceSlider;
    [SerializeField] private TextMeshProUGUI experienceText;

    [SerializeField] private TextMeshProUGUI levelText; // ⭐️ 新增：等级文本

    [Header("升级设置")]
    // 经验值不再是固定的，所以移除 experienceToLevelUp 的 [Tooltip] 和 public
    // [Tooltip("升级到下一级所需的总经验值")]
    // public int experienceToLevelUp = 100; 

    private PlayerState _playerState;
    private PlayerCharacter _playerCharacter;

    void Start()
    {
        _playerState = (PlayerState)Player.GetInstance().PlayerState;
        _playerCharacter = FindObjectOfType<PlayerCharacter>();
        if (experienceSlider == null)
        {
            experienceSlider = GetComponentInChildren<UnityEngine.UI.Slider>();
        }
        if (experienceText == null)
        {
            experienceText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (_playerState == null || _playerCharacter == null)
        {
            UnityEngine.Debug.LogError("ExperienceBarUI: 无法找到 PlayerState 或 PlayerCharacter！");
            this.enabled = false;
        }

        UpdateDisplay();
    }

    void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_playerState == null || _playerCharacter == null || experienceSlider == null || experienceText == null || levelText == null)
        {
            return;
        }

        int currentLevel = _playerState.Level;
        // ⭐️ 获取下一级所需经验值
        int expRequired = _playerCharacter.CalculateExpRequired(currentLevel + 1);
        // ⭐️ 获取当前等级起始时的经验值
        int expStart = _playerCharacter.CalculateExpRequired(currentLevel);

        // 当前等级内获得的经验值 (用于进度条)
        int currentExpProgress = _playerState.Experience - expStart;
        // 当前等级总共需要的经验值
        int totalExpRequiredForLevel = expRequired - expStart;

        // ⭐️ 更新Slider
        experienceSlider.maxValue = totalExpRequiredForLevel;
        experienceSlider.value = currentExpProgress;

        // ⭐️ 更新经验文本
        experienceText.text = $"{currentExpProgress} / {totalExpRequiredForLevel}";

        // ⭐️ 更新等级文本
        levelText.text = $"Lv.{currentLevel}";
    }
}