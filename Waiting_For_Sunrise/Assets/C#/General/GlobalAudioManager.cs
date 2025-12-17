using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GlobalAudioManager : MonoBehaviour
{
    // 1. 混音器引用
    [Header("混音器和音轨")]
    [Tooltip("拖拽你的 MainMixer 资产")]
    [SerializeField] private AudioMixer mainMixer;
    [Tooltip("挂载在 AudioManager 上的 AudioSource 组件，用于播放BGM")]
    [SerializeField] private AudioSource bgmAudioSource;

    // ⭐️ 新增：暴露 SFX 混音器组给其他脚本
    [Header("混音器组引用")]
    [Tooltip("SFX 组引用，在 Awake 中查找并赋值")]
    public AudioMixerGroup SFXGroup;

    // 2. 场景音乐 Clips (在 Inspector 中配置)
    [Header("场景音乐配置")]
    [Tooltip("主菜单的音乐")]
    [SerializeField] private AudioClip mainMenuBGM;
    [Tooltip("night 的音乐")]
    [SerializeField] private AudioClip nightBGM;
    [Tooltip("shop 的音乐")]
    [SerializeField] private AudioClip ShopBGM;

    // 3. 混音器参数名称 (必须与步骤一中 Exposed Parameters 的名称完全一致)
    private const string BGM_PARAM = "BGM_Volume";
    private const string SFX_PARAM = "SFX_Volume";

    private static GlobalAudioManager instance;
    public static GlobalAudioManager Instance => instance; // ⭐️ 单例访问器

    void Awake()
    {
        // 实现单例模式：确保只有一个 AudioManager 实例
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 确保 AudioSource 存在
        if (bgmAudioSource == null)
        {
            bgmAudioSource = gameObject.AddComponent<AudioSource>();
        }

        // 确保 bgmAudioSource 输出到 BGM 组，并查找 SFX 组
        if (mainMixer != null)
        {
            // 确保 BGM AudioSource 输出到 BGM 组
            AudioMixerGroup[] bgmGroups = mainMixer.FindMatchingGroups("BGM");
            if (bgmGroups.Length > 0)
            {
                bgmAudioSource.outputAudioMixerGroup = bgmGroups[0];
            }

            // ⭐️ 核心：查找并存储 SFX 组的引用
            AudioMixerGroup[] sfxGroups = mainMixer.FindMatchingGroups("SFX");
            if (sfxGroups.Length > 0)
            {
                SFXGroup = sfxGroups[0];
                // Debug.Log("AudioManager: ✅ 成功获取 SFX 混音器组。");
            }
            else
            {
                Debug.LogError("AudioManager: ❌ 未找到名为 'SFX' 的混音器组! 武器音效将不受控制。");
            }
        }

        // 注册场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("AudioManager: ✅ 初始化完成，监听场景变化。");

        // 初始化音量（默认最大）
        SetBGMVolume(1f);
        SetSFXVolume(1f);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // --- 场景加载回调：根据场景名称切换 BGM ---

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AudioClip newClip = null;
        string sceneName = scene.name;

        if (sceneName.Contains("MainMenu"))
        {
            newClip = mainMenuBGM;
        }
        else if (sceneName.Contains("Night"))
        {
            newClip = nightBGM;
        }
        else if (sceneName.Contains("Shopsss")) // 假设您修正了 Shop 音乐的播放
        {
            newClip = ShopBGM;
        }
        else // 默认战斗场景或其他场景
        {
            newClip = nightBGM;
        }

        PlayBGM(newClip);
    }

    // --- 核心播放方法 ---

    public void PlayBGM(AudioClip clip)
    {
        if (bgmAudioSource == null || clip == null) return;

        if (bgmAudioSource.clip == clip && bgmAudioSource.isPlaying)
        {
            return; // 已经是当前音乐，不重复播放
        }

        Debug.Log($"AudioManager: 🎶 切换BGM到: {clip.name}");
        bgmAudioSource.clip = clip;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }

    // --- 核心音量控制方法 ---

    // 接受 0.0 到 1.0 的浮点数作为音量滑块输入
    public void SetBGMVolume(float volume)
    {
        SetVolume(BGM_PARAM, volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetVolume(SFX_PARAM, volume);
    }

    private void SetVolume(string exposedParameter, float volume)
    {
        if (mainMixer == null) return;

        // Mixer 的 Volume 参数使用对数刻度 (logarithmic scale)
        float dB = (volume > 0.0001f) ? Mathf.Log10(volume) * 20f : -80f;

        mainMixer.SetFloat(exposedParameter, dB);
        // Debug.Log($"AudioManager: 调整 {exposedParameter} 音量到 {volume:F2} ({dB:F2} dB)");
    }


    /// <summary>
    /// 停止当前正在播放的背景音乐
    /// </summary>
    public void StopBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
            Debug.Log("AudioManager:  背景音乐已停止。");
        }
    }

    /// <summary>
    /// 暂停当前正在播放的背景音乐
    /// </summary>
    public void PauseBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Pause();
            Debug.Log("AudioManager:  背景音乐已暂停。");
        }
    }
}