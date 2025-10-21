using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    // 参数哈希值
    private readonly int moveXHash = Animator.StringToHash("moveX");
    private readonly int moveYHash = Animator.StringToHash("moveY");
    private readonly int isMovingHash = Animator.StringToHash("isMoving");
    private readonly int lastMoveXHash = Animator.StringToHash("lastMoveX");
    private readonly int lastMoveYHash = Animator.StringToHash("lastMoveY");
    private readonly int rollTriggerHash = Animator.StringToHash("Roll"); // 新增翻滚触发器


    // 新增一个变量来存储Roll状态的哈希值
    private int rollStateHash = Animator.StringToHash("Roll State");

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // 如果正在翻滚，则停止更新所有移动/站立动画参数
        if (playerMovement.IsRolling)
        {
            return;
        }

        Vector2 moveInput = playerMovement.MoveInput;
        bool isMoving = moveInput.sqrMagnitude > 0;

        animator.SetBool(isMovingHash, isMoving);

        if (isMoving)
        {
            animator.SetFloat(moveXHash, moveInput.x);
            animator.SetFloat(moveYHash, moveInput.y);
            animator.SetFloat(lastMoveXHash, moveInput.x);
            animator.SetFloat(lastMoveYHash, moveInput.y);
        }

        // 处理精灵翻转
        if (moveInput.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    // 公共方法，供 PlayerMovement 脚本调用
    public void TriggerRollAnimation()
    {
        // 获取当前动画状态信息
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // 只有当当前动画不是 "Roll State" 时，才允许触发翻滚
        if (!stateInfo.IsName("Roll State"))
        {
            animator.SetTrigger(rollTriggerHash);
        }
    }
}