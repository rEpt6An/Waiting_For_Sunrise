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
    private Transform visualTf; // ⭐️ 新增：缓存视觉Transform
    private int currentClip;
    private bool isReloading = false;
    private float attackCooldownTimer = 0f;

    // 攻击类型常量 (与 WeaponData 中的 int 值保持一致)
    private const int ATTACK_TYPE_MELEE = 0;
    private const int ATTACK_TYPE_RANGED = 1;

    private void Start()
    {
        // 1. 初始化武器图片和视觉 Transform
        SetupWeaponVisuals();

        // 2. 初始化弹夹
        if (currentWeaponData != null)
        {
            currentClip = currentWeaponData.clip;
        }
    }

    private void Update()
    {
        if (currentWeaponData == null) return;

        // --- 1. 攻击冷却计时 ---
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        // --- 2. 武器朝向鼠标和角色翻转 ---
        HandleWeaponAimingAndFlip();

        // --- 3. 自动装弹 ---
        if (currentWeaponData.attackType == ATTACK_TYPE_RANGED && currentClip <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        // --- 4. 尝试攻击 (改为按住鼠标连发) ---
        // ⭐️ 核心修改: GetMouseButtonDown(0) -> GetMouseButton(0)
        if (Input.GetMouseButton(0))
        {
            TryAttack();
        }
    }

    // --- 新增：处理武器瞄准和垂直翻转 ---
    private void HandleWeaponAimingAndFlip()
    {
        if (visualTf == null) return;

        // 1. 计算鼠标方向
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 2. 武器旋转
        // 武器视觉对象绕 Z 轴旋转，朝向鼠标
        visualTf.rotation = Quaternion.Euler(0, 0, angle);

        // 3. 角色垂直翻转逻辑 (基于 W/S 输入)
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 newScale = visualTf.localScale;

        if (verticalInput > 0) // 按 W 向上走
        {
            // ⭐️ 向上时设置为 1
            newScale.y = 1f;
        }
        else if (verticalInput < 0) // 按 S 向下走
        {
            // ⭐️ 向下时设置为 -1
            newScale.y = -1f;
        }
        // 如果没有垂直输入，保持当前状态

        visualTf.localScale = newScale;
    }


    // --- 可视化设置 ---
    private void SetupWeaponVisuals()
    {
        GameObject visualGO = new GameObject("Weapon Visual");
        visualGO.transform.SetParent(transform);
        visualTf = visualGO.transform; // 缓存 Transform
        visualTf.localPosition = visualOffset;

        weaponImageRenderer = visualGO.AddComponent<SpriteRenderer>();
        if (currentWeaponData.image != null)
        {
            weaponImageRenderer.sprite = currentWeaponData.image;
        }
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
                    Debug.Log("弹夹为空，正在装弹中或等待装弹...");
                    return;
                }
                currentClip--; // 消耗弹药
            }

            // 攻击逻辑
            PerformAttack();

            // 设置攻击冷却
            // ⚠️ 建议 PlayerCharacter 中使用玩家攻速加成来修改冷却时间
            attackCooldownTimer = 1f / currentWeaponData.baseAttackSpeed;

            // 触发武器动画 (近战和远程都使用相同的旋转动画)
            StartCoroutine(WeaponAttackAnimation());
        }
    }

    private void PerformAttack()
    {
        float finalDamage = currentWeaponData.baseDamage; // 伤害计算应在 PlayerCharacter 中，这里仅作为示例

        // 1. 计算攻击方向 (使用视觉对象的旋转方向)
        Vector3 direction = visualTf.right; // 武器视觉对象的 X 轴指向即为攻击方向

        // 2. 实例化攻击碰撞体 (使用武器视觉对象的位置)
        // 如果远程子弹需要沿视觉方向发射，则应使用 visualTf.rotation 
        GameObject attackGO = Instantiate(weaponAttackPrefab, visualTf.position, visualTf.rotation);

        // 3. 初始化 WeaponAttack 脚本
        //WeaponAttack attackComponent = attackGO.GetComponent<WeaponAttack>();
        //if (attackComponent != null)
        //{
        //    attackComponent.Initialize(finalDamage, currentWeaponData.repel, transform.position); // 攻击发起点仍用玩家位置
        //}

        // 4. ⭐️ 远程武器特有：施加初始速度
        if (currentWeaponData.attackType == ATTACK_TYPE_RANGED)
        {
            Rigidbody2D rb = attackGO.GetComponent<Rigidbody2D>();
            // 使用 visualTf.right (即武器朝向) 作为子弹方向
            if (rb != null && currentWeaponData.projectileSpeed > 0)
            {
                rb.velocity = direction * currentWeaponData.projectileSpeed;
            }
        }
    }

    // --- 装弹逻辑 (保持不变) ---
    IEnumerator Reload()
    {
        if (isReloading) yield break;

        isReloading = true;
        Debug.Log($"[{currentWeaponData.weaponName}] 开始装弹...");

        yield return new WaitForSeconds(currentWeaponData.cliptime);

        currentClip = currentWeaponData.clip;
        isReloading = false;
        Debug.Log($"[{currentWeaponData.weaponName}] 装弹完成！当前弹夹: {currentClip}");
    }

    // --- 武器攻击动画 (统一为旋转动画) ---
    IEnumerator WeaponAttackAnimation()
    {
        if (visualTf == null || currentWeaponData == null) yield break;

        // ⭐️ 使用数据驱动的旋转动画 (近战和远程现在都使用这个)
        float angle = currentWeaponData.rotationAngle;
        float duration = currentWeaponData.animationDuration / 2f; // 往或返的单程时间

        // 保存初始状态，以便在动画结束后恢复，防止和鼠标瞄准冲突
        Quaternion originalRotation = visualTf.localRotation;

        // 计算旋转终点：在当前面向方向上额外旋转
        Quaternion endRotation = originalRotation * Quaternion.Euler(0, 0, angle);

        // 前摇/攻击旋转
        float timer = 0f;
        while (timer < duration)
        {
            // 使用局部旋转进行动画
            visualTf.localRotation = Quaternion.Lerp(originalRotation, endRotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        // 回位动画
        timer = 0f;
        while (timer < duration)
        {
            visualTf.localRotation = Quaternion.Lerp(endRotation, originalRotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        // 动画结束，不强制设置回 localRotation，让 HandleWeaponAimingAndFlip 接管，实现平滑过渡
    }
}