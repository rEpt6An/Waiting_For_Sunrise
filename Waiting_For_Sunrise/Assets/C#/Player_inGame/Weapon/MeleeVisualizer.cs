// MeleeVisualizer.cs
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class MeleeVisualizer : MonoBehaviour
{
    // 闪烁时间
    private float flashDuration = 0.05f;
    // 轨迹颜色
    private Color flashColor = Color.white;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        // 确保 SpriteRenderer 是可见的
        if (sr.sprite == null)
        {
            // 假设你有一个小的白色方块 sprite
            // 如果没有，可以临时创建一个：
            // sr.sprite = Resources.Load<Sprite>("WhiteSquareSprite"); 

            // 为了简化，我们只控制颜色和透明度
        }
        StartCoroutine(FlashEffect());
    }

    IEnumerator FlashEffect()
    {
        // 设置初始状态
        Color originalColor = sr.color;
        sr.color = flashColor;

        // 渐隐效果
        float timer = 0f;
        while (timer < flashDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, timer / flashDuration);
            sr.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        // 确保完全透明
        sr.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0f);

        // 由于 PlayerCharacter.PerformAttack 已经设置了 Destroy(gameObject, 0.3f)，
        // 攻击碰撞体会在稍后被销毁。
    }
}