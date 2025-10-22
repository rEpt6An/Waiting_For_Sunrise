using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimator))] // 确保PlayerAnimator脚本存在
public class PlayerMovement : MonoBehaviour

{

    public static Transform Instance { get; private set; }

    [Header("移动属性")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("翻滚属性")]
    [SerializeField] private float rollDistance = 0.01f; // 翻滚的固定距离
    [SerializeField] private float rollDuration = 0.2f; // 翻滚持续时间
    [SerializeField] private float rollCooldown = 0.1f; // 翻滚冷却时间 (Roll_cd)

    private Rigidbody2D rb;
    private PlayerAnimator playerAnimator; // 对动画脚本的引用
    private Vector2 moveInput;
    private Vector2 lastMoveDirection; // 记录上一次的移动方向

    private bool isRolling = false;
    private float rollCooldownTimer = 0f;

    // 公共属性，让其他脚本可以安全地读取输入和状态
    public Vector2 MoveInput => moveInput;
    public bool IsRolling => isRolling;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this.transform;

        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        // 1. 处理冷却计时
        if (rollCooldownTimer > 0)
        {
            rollCooldownTimer -= Time.deltaTime;
        }

        // 2. 如果正在翻滚，则不接受任何新输入
        if (isRolling)
        {
            return;
        }

        // 3. 处理常规移动输入
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (moveInput.sqrMagnitude > 0)
        {
            lastMoveDirection = moveInput;
        }

        // 4. 处理翻滚输入
        if (Input.GetKeyDown(KeyCode.Space) && rollCooldownTimer <= 0)
        {
            StartCoroutine(PerformRoll());
        }
    }

    void FixedUpdate()
    {
        // 只有在不翻滚的时候才应用常规移动
        if (!isRolling)
        {
            rb.velocity = moveInput * moveSpeed;
        }
    }

    private IEnumerator PerformRoll()
    {
        // --- 翻滚开始 ---
        isRolling = true;
        rollCooldownTimer = rollCooldown;

        // 触发动画
        playerAnimator.TriggerRollAnimation();

        // 确定翻滚方向（如果没有移动，就用上一次的方向）
        Vector2 rollDirection = lastMoveDirection.sqrMagnitude > 0 ? lastMoveDirection : Vector2.right;

        // --- 翻滚过程 ---
        float rollSpeed = rollDistance / rollDuration;
        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            rb.velocity = rollDirection * rollSpeed;
            elapsedTime += Time.fixedDeltaTime; // 使用FixedUpdate的时间步长
            yield return new WaitForFixedUpdate(); // 在物理循环中等待
        }

        // --- 翻滚结束 ---
        rb.velocity = Vector2.zero; // 翻滚结束后立即停止
        isRolling = false;
    }
}