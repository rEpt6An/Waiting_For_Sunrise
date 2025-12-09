using UnityEngine;
using System.Collections;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponImageRenderer;
    [SerializeField] private WeaponData currentWeaponData;
    [SerializeField] private Transform visualTransform;

    // 远程武器状态
    private int currentClip;
    private bool isReloading = false;

    // 攻击类型常量
    private const int ATTACK_TYPE_MELEE = 0;
    private const int ATTACK_TYPE_RANGED = 1;

    public void Initialize(WeaponData data)
    {
        currentWeaponData = data;

        if (weaponImageRenderer == null) weaponImageRenderer = GetComponent<SpriteRenderer>();
        if (visualTransform == null) visualTransform = transform;

        if (currentWeaponData != null)
        {
            // 设置武器图片
            if (weaponImageRenderer != null)
            {
                weaponImageRenderer.sprite = currentWeaponData.image;
            }
            currentClip = currentWeaponData.clip;
        }
    }
    private void Update()

    {

        // 尝试自动装弹 (如果远程武器没弹药且不在装弹中)

        if (currentWeaponData != null &&

            currentWeaponData.attackType == ATTACK_TYPE_RANGED &&

            currentClip <= 0 &&

            !isReloading)

        {

            StartCoroutine(Reload());

        }

    }

    public bool ConsumeClip()

    {

        if (currentWeaponData.attackType == ATTACK_TYPE_RANGED)

        {

            if (currentClip > 0)

            {

                currentClip--;

                return true;

            }

            return false; // 弹药不足

        }

        return true; // 近战武器永远有弹药

    }

    IEnumerator Reload()

    {

        if (isReloading) yield break;



        isReloading = true;

        Debug.Log("开始装弹...");



        yield return new WaitForSeconds(currentWeaponData.cliptime);



        currentClip = currentWeaponData.clip;

        isReloading = false;

        Debug.Log("装弹完成！");

    }

    /// <summary>
    /// 供 PlayerCharacter 调用的攻击触发动画方法
    /// </summary>
    public void TriggerAttackAnimation(int attackType)
    {
        if (!isReloading && currentWeaponData != null)
        {
            // 可以在这里播放攻击音效
            if (currentWeaponData.attackSound != null)
            {
                // 假设有一个 AudioSource 组件来播放声音
                // GetComponent<AudioSource>()?.PlayOneShot(currentWeaponData.attackSound);
            }
            StartCoroutine(WeaponAttackAnimation(attackType));
        }
    }

    // --- 武器攻击动画 (使用 WeaponData 参数) ---
    IEnumerator WeaponAttackAnimation(int attackType)
    {
        if (visualTransform == null || currentWeaponData == null) yield break;

        // ⭐️ 从 WeaponData 中读取动画持续时间
        float duration = currentWeaponData.animationDuration / 2f; // 往或返的单程时间

        if (attackType == ATTACK_TYPE_MELEE)
        {
            float angle = currentWeaponData.rotationAngle; // ⭐️ 从数据中获取旋转角度
            Quaternion startRotation = visualTransform.localRotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, angle);

            // 前摇/攻击旋转
            float timer = 0f;
            while (timer < duration)
            {
                visualTransform.localRotation = Quaternion.Lerp(startRotation, endRotation, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }

            // 回位旋转
            timer = 0f;
            while (timer < duration)
            {
                visualTransform.localRotation = Quaternion.Lerp(endRotation, startRotation, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            visualTransform.localRotation = startRotation;
        }
        else if (attackType == ATTACK_TYPE_RANGED)
        {
            Vector3 originalPos = visualTransform.localPosition;
            float shakeIntensity = currentWeaponData.shakeIntensity; // ⭐️ 从数据中获取抖动强度
            float shakeDuration = currentWeaponData.animationDuration; // 抖动持续时间

            for (float t = 0; t < shakeDuration; t += Time.deltaTime)
            {
                float x = Random.Range(-1f, 1f) * shakeIntensity;
                float y = Random.Range(-1f, 1f) * shakeIntensity;
                visualTransform.localPosition = originalPos + new Vector3(x, y, 0);
                yield return null;
            }
            visualTransform.localPosition = originalPos;
        }
    }


    /// <summary>
    /// 检查武器是否正在装弹
    /// </summary>
    public bool IsReloading()
    {
        return isReloading;
    }


}