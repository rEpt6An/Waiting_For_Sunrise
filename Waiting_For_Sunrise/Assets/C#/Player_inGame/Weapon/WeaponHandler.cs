// WeaponHandler.cs
using UnityEngine;
using System.Collections;

public class WeaponHandler : MonoBehaviour
{
    [Header("配置")]
    public WeaponData currentWeaponData;
    [Tooltip("挂载了 WeaponAttack.cs 的碰撞体预制件")]
    public GameObject weaponAttackPrefab;
    [Tooltip("武器图片显示的偏移量")]
    public Vector3 visualOffset = new Vector3(0.5f, 0, 0);

    // 运行时数据
    private SpriteRenderer weaponImageRenderer;
    private int currentClip;
    private bool isReloading = false;
    private float attackCooldownTimer = 0f;

    // 攻击类型常量 (与 WeaponData 中的 int 值保持一致)
    private const int ATTACK_TYPE_MELEE = 0;
    private const int ATTACK_TYPE_RANGED = 1;

    private void Start()
    {
        // 1. 初始化武器图片
        SetupWeaponVisuals();

        // 2. 初始化弹夹
        currentClip = currentWeaponData.clip;
    }

    private void Update()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        // 尝试自动装弹 (如果远程武器没弹药且不在装弹中)
        if (currentWeaponData.attackType == ATTACK_TYPE_RANGED && currentClip <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        // 示例：按鼠标左键尝试攻击
        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
    }

    // --- 可视化设置 ---
    private void SetupWeaponVisuals()
    {
        // 创建一个子对象来放置武器图片
        GameObject visualGO = new GameObject("Weapon Visual");
        visualGO.transform.SetParent(transform);
        visualGO.transform.localPosition = visualOffset;

        weaponImageRenderer = visualGO.AddComponent<SpriteRenderer>();
        if (currentWeaponData.image != null)
        {
            weaponImageRenderer.sprite = currentWeaponData.image;
        }
        // 确保武器在角色前面显示 (例如设置一个更高的 Sorting Order)
        weaponImageRenderer.sortingOrder = 10;
    }

    // --- 攻击核心逻辑 ---
    public void TryAttack()
    {
        if (attackCooldownTimer <= 0 && !isReloading)
        {
            // 检查弹夹 (仅对远程武器有效)
            if (currentWeaponData.attackType == ATTACK_TYPE_RANGED)
            {
                if (currentClip <= 0)
                {
                    // 没有子弹，尝试装弹（或者直接返回等待自动装弹）
                    Debug.Log("弹夹为空，正在装弹中或等待装弹...");
                    return;
                }
                currentClip--; // 消耗弹药
            }

            // 攻击逻辑
            PerformAttack();

            // 设置攻击冷却
            attackCooldownTimer = currentWeaponData.baseAttackSpeed;

            // 触发武器动画
            StartCoroutine(WeaponAttackAnimation());
        }
    }

    private void PerformAttack()
    {
        // ⚠️ 实际游戏应在这里计算最终伤害，这里使用 baseDamage 作为示例
        float finalDamage = currentWeaponData.baseDamage;

        // 实例化攻击碰撞体 (使用角色的位置作为攻击发起点)
        GameObject attackGO = Instantiate(weaponAttackPrefab, transform.position + visualOffset, Quaternion.identity);

        // 获取 WeaponAttack 脚本并初始化
        WeaponAttack attackComponent = attackGO.GetComponent<WeaponAttack>();
        if (attackComponent != null)
        {
            // 传递伤害值，击退强度，和攻击发起位置 (玩家的中心位置)
            attackComponent.Initialize(finalDamage, currentWeaponData.repel, transform.position);
        }
    }


    // --- ⭐️ 装弹逻辑 (仅远程武器) ---
    IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;
        Debug.Log($"[{currentWeaponData.weaponName}] 开始装弹...");

        // 等待装弹时间
        yield return new WaitForSeconds(currentWeaponData.cliptime);

        // 装弹完成
        currentClip = currentWeaponData.clip;
        isReloading = false;
        Debug.Log($"[{currentWeaponData.weaponName}] 装弹完成！当前弹夹: {currentClip}");
    }

    // ---  武器攻击动画 ---
    IEnumerator WeaponAttackAnimation()
    {
        Transform visualTf = weaponImageRenderer.transform;

        if (currentWeaponData.attackType == ATTACK_TYPE_MELEE)
        {
            //  近战动画：旋转 90 度
            float duration = 0.1f;
            Quaternion startRotation = visualTf.localRotation;
            // 旋转 90 度，注意这个旋转是相对于父物体 (即角色) 的局部旋转
            Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, 90);

            // 执行旋转动画
            float timer = 0f;
            while (timer < duration)
            {
                visualTf.localRotation = Quaternion.Lerp(startRotation, endRotation, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            // 确保完全到达终点
            visualTf.localRotation = endRotation;

            // 等待一小段时间 (可选)
            yield return new WaitForSeconds(0.05f);

            // 回位动画
            timer = 0f;
            while (timer < duration)
            {
                visualTf.localRotation = Quaternion.Lerp(endRotation, startRotation, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            // 确保回到原位
            visualTf.localRotation = startRotation;
        }
        else if (currentWeaponData.attackType == ATTACK_TYPE_RANGED)
        {
            //  远程动画：抖动效果
            Vector3 originalPos = visualTf.localPosition;
            float shakeIntensity = 0.05f; // 抖动幅度
            float shakeDuration = 0.1f; // 抖动持续时间

            for (float t = 0; t < shakeDuration; t += Time.deltaTime)
            {
                // 生成随机偏移量
                float x = Random.Range(-1f, 1f) * shakeIntensity;
                float y = Random.Range(-1f, 1f) * shakeIntensity;
                visualTf.localPosition = originalPos + new Vector3(x, y, 0);
                yield return null;
            }
            // 确保动画结束后回到原位
            visualTf.localPosition = originalPos;
        }
    }
}