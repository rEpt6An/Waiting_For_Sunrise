using UnityEngine;
using UnityEngine.UI; // 必须引用

public class SettingsManager : MonoBehaviour
{
    [Header("UI 滑块引用")]
    [Tooltip("拖拽 BGM 音量滑块")]
    [SerializeField] private Slider bgmSlider;
    [Tooltip("拖拽 SFX 音量滑块")]
    [SerializeField] private Slider sfxSlider;

    private GlobalAudioManager audioManagerInstance;

    void Start()
    {
        // 查找或等待 GlobalAudioManager 实例
        audioManagerInstance = FindObjectOfType<GlobalAudioManager>();

        if (audioManagerInstance == null)
        {
            Debug.LogError("SettingsManager: ❌ 场景中找不到 GlobalAudioManager 实例!");
            return;
        }

        // 确保滑块在 Start 时就更新一次音量
        if (bgmSlider != null)
        {
            // 初始化滑块的值 (可选：可以从 PlayerPrefs 加载上次保存的值)
            bgmSlider.value = 1f;
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            OnBGMVolumeChanged(bgmSlider.value); // 立即应用初始值
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = 1f;
            sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            OnSFXVolumeChanged(sfxSlider.value); // 立即应用初始值
        }
    }

    // --- 滑块回调方法 ---

    public void OnBGMVolumeChanged(float value)
    {
        if (audioManagerInstance != null)
        {
            audioManagerInstance.SetBGMVolume(value);
        }
    }

    public void OnSFXVolumeChanged(float value)
    {
        // 您的想法：音效就是除了背景音乐之外的所有声音
        // 所以我们用 SFX 参数控制所有游戏内的音效（攻击、跳跃、UI点击等）
        if (audioManagerInstance != null)
        {
            audioManagerInstance.SetSFXVolume(value);
        }
    }
}