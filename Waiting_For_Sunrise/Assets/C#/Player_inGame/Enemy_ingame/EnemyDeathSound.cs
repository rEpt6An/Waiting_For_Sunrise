using UnityEngine;

// RequireComponent 确保这个脚本只有在挂载了 EnemyController 的对象上才能正常工作
[RequireComponent(typeof(EnemyController))]
public class EnemyDeathSound : MonoBehaviour
{
    // 拖拽你的死亡音效文件到 Inspector
    [Tooltip("怪物死亡时播放的音效片段")]
    [SerializeField] private AudioClip deathSoundClip;

    // 音源组件，用于播放音效
    private AudioSource audioSource;
    private EnemyController enemyController;

    // 用于标记音效是否已经播放，防止重复播放或被销毁前播放多次
    private bool soundPlayed = false;

    void Awake()
    {
        // 确保获取了 EnemyController 和 AudioSource 组件
        enemyController = GetComponent<EnemyController>();

        // 尝试获取 AudioSource 组件，如果没有就添加一个
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 设置 AudioSource 的基础属性
        audioSource.playOnAwake = false;
        audioSource.clip = deathSoundClip;
    }

    // ⭐️ 核心逻辑：在 Update 中持续检查怪物的生命状态
    void Update()
    {
        // 我们假设 EnemyController 中有一个 protected bool isDead 或类似的状态
        // 由于我们没有父类的完整代码，我们通过检查怪物的血量来判断它是否死亡。
        // *注意：如果你的 EnemyController 中有 public 的 Die() 方法，可以考虑使用事件系统监听。
        // 这里使用更通用的方式：检查组件是否存在或血量是否为零 (如果 EnemyController 暴露了血量)

        // 检查 EnemyController 组件是否被销毁 (即怪物调用了 base.Die() 并销毁了自身)
        if (enemyController == null && !soundPlayed)
        {
            // 这种检查方法不准确，因为脚本的生命周期会在销毁前结束。
            // 更好的方法是使用 OnDestroy。
            return;
        }

        // 另一种常见做法：检查怪物是否应该死亡
        // 由于我们无法直接访问 protected 的 currentHealth，我们必须依赖 Die() 方法或 OnDestroy。
    }

    // ⭐️ 在怪物被销毁前调用此方法
    void OnDestroy()
    {
        // 当挂载此脚本的游戏对象被销毁时，这个方法会被调用。
        // 这通常发生在 EnemyController.Die() 中调用 Destroy(gameObject) 之后。

        if (soundPlayed) return;

        if (deathSoundClip != null)
        {
            // 使用 AudioSource.PlayClipAtPoint 是最好的方法，因为它会在世界坐标播放一次，
            // 不需要依赖原有的 AudioSource 实例，防止原对象被销毁时音效中断。

            // ⚠️ 重要提示：AudioSource.PlayClipAtPoint 不会受 AudioSource 组件的设置影响。
            // 如果你需要音量、3D设置等，需要调整 AudioSource.PlayClipAtPoint 的重载参数。
            AudioSource.PlayClipAtPoint(deathSoundClip, transform.position);

            soundPlayed = true;
            Debug.Log($"EnemyDeathSound: 🔊 播放死亡音效: {deathSoundClip.name}");
        }
    }
}