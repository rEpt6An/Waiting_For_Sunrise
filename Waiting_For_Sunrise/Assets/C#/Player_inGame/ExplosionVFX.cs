// ExplosionVFX.cs
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeDuration = 0.5f; // 特效持续时间
    private float startTime;
    private bool isFading = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("ExplosionVFX 需要一个 SpriteRenderer 组件！");
            Destroy(gameObject);
        }
    }

    public void StartFadeOutAndDestroy()
    {
        startTime = Time.time;
        isFading = true;
    }

    void Update()
    {
        if (!isFading) return;

        float timeElapsed = Time.time - startTime;
        float normalizedTime = timeElapsed / fadeDuration;

        // 从 1.0 淡出到 0.0
        float alpha = Mathf.Lerp(1f, 0f, normalizedTime);

        // 应用新的颜色（Alpha 值）
        Color tempColor = spriteRenderer.color;
        tempColor.a = alpha;
        spriteRenderer.color = tempColor;

        if (normalizedTime >= 1f)
        {
            // 淡出结束，销毁特效对象
            Destroy(gameObject);
        }
    }
}