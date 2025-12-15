using UnityEngine;
using System.Collections;

public class WeaponAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer weaponImageRenderer;
    // 武器的视觉Transform，挂载在玩家上的子对象（例如：Weapon Visual）
    [SerializeField] private WeaponData currentWeaponData;
    [SerializeField] private Transform visualTransform;

    // 远程武器状态
    public int currentClip;
    private bool isReloading = false;

    // 攻击类型常量
    private const int ATTACK_TYPE_MELEE = 0;
    private const int ATTACK_TYPE_RANGED = 1;

    // ⭐️ 音效组件
    private AudioSource audioSource;

    // 缓存初始旋转，用于动画结束恢复
    private Quaternion initialLocalRotation;


    /// <summary>
    /// 供 UI 调用的，获取当前弹夹剩余数量
    /// </summary>
    public int GetCurrentClip()
    {
        return currentClip;
    }

    public void Initialize(WeaponData data)
    {
        currentWeaponData = data;

        // 确保获取组件，如果 Inspector 中未赋值
        if (weaponImageRenderer == null) weaponImageRenderer = GetComponent<SpriteRenderer>();
        if (visualTransform == null) visualTransform = transform;


        // ⭐️ 核心：获取或添加 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false; // 避免自动播放
        }

        // ⭐️ 核心：将 AudioSource 连接到 SFX 混音器组
        if (GlobalAudioManager.Instance != null && GlobalAudioManager.Instance.SFXGroup != null)
        {
            audioSource.outputAudioMixerGroup = GlobalAudioManager.Instance.SFXGroup;
        }
        else
        {
            Debug.LogWarning("WeaponAnimator: ⚠️ 无法连接到 SFX 混音器组，请检查 GlobalAudioManager 是否已初始化，且混音器中存在名为 'SFX' 的组。");
        }


        // 缓存视觉Transform的初始局部旋转
        initialLocalRotation = visualTransform.localRotation;

        if (currentWeaponData != null)
        {
            if (weaponImageRenderer != null)
            {
                weaponImageRenderer.sprite = currentWeaponData.image;
            }
            currentClip = currentWeaponData.clip;
        }
    }

    private void Update()
    {
        if (currentWeaponData == null) return;

        // --- 1. 武器跟随鼠标转向 ---
        HandleWeaponAiming();

        // --- 2. 尝试自动装弹 ---
        if (currentWeaponData.attackType == ATTACK_TYPE_RANGED && currentClip <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }


        // --- 3. 手动装弹 (按 R 键) ---
        if (Input.GetKeyDown(KeyCode.R) && currentWeaponData.attackType == ATTACK_TYPE_RANGED && !isReloading && currentClip < currentWeaponData.clip)
        {
            // 如果不在装弹中，且弹夹未满，则开始手动装弹
            StartCoroutine(Reload());
        }
    }

    // --- 新增：处理武器瞄准 (跟随鼠标) ---
    private void HandleWeaponAiming()
    {
        if (visualTransform == null) return;

        // 1. 获取鼠标在世界坐标系的位置
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // 2. 计算方向和角度
        Vector2 direction = (mousePosition - visualTransform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 3. 旋转武器视觉对象，使其朝向鼠标
        visualTransform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public bool ConsumeClip()
    {
        if (currentWeaponData == null) return true;

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

    public bool IsReloading()
    {
        return isReloading;
    }

    // --- 装弹逻辑 (添加装弹动画) ---
    IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;
        Debug.Log("开始装弹...");

        // ⭐️ 装弹动画：旋转 360 度
        if (visualTransform != null)
        {
            Quaternion startRot = visualTransform.localRotation;
            Quaternion endRot = startRot * Quaternion.Euler(0, 0, 360f); // 局部旋转 360 度
            float reloadTime = currentWeaponData.cliptime;
            float timer = 0f;

            while (timer < reloadTime)
            {
                // 使用 Lerp 使旋转平滑
                visualTransform.localRotation = Quaternion.Lerp(startRot, endRot, timer / reloadTime);
                timer += Time.deltaTime;
                yield return null;
            }
            // 确保回到初始状态
            visualTransform.localRotation = startRot;
        }


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
            // ⭐️ 核心：播放音效
            if (audioSource != null && currentWeaponData.attackSound != null)
            {
                audioSource.PlayOneShot(currentWeaponData.attackSound);
            }

            // 确保停止之前的动画，避免冲突
            StopCoroutine("WeaponAttackAnimationCoroutine");
            StartCoroutine(WeaponAttackAnimationCoroutine(attackType));
        }
    }

    // --- 武器攻击动画 (统一为旋转动画) ---
    IEnumerator WeaponAttackAnimationCoroutine(int attackType)
    {
        if (visualTransform == null || currentWeaponData == null) yield break;

        // ⭐️ 统一使用近战的旋转动画逻辑
        float duration = currentWeaponData.animationDuration / 2f;
        float angle = currentWeaponData.rotationAngle;

        // 使用当前的旋转作为起始旋转，以便在瞄准时也能正常进行攻击动画
        Quaternion startRotation = visualTransform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, angle);

        // 前摇/攻击旋转
        float timer = 0f;
        while (timer < duration)
        {
            // 攻击动画应该使用世界旋转 (rotation) 以覆盖鼠标瞄准的方向
            visualTransform.rotation = Quaternion.Lerp(startRotation, endRotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        // 回位旋转
        timer = 0f;
        while (timer < duration)
        {
            visualTransform.rotation = Quaternion.Lerp(endRotation, startRotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        // 动画结束，不强制设置 rotation，让 Update 中的 HandleWeaponAiming 接管
        // visualTransform.rotation = startRotation; 
    }
}