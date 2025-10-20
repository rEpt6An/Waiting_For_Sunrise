using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))] // 确保PlayerMovement脚本存在
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    // 使用 Animator.StringToHash 可以提高性能，避免在Update中反复进行字符串比较
    private readonly int isMovingHash = Animator.StringToHash("isMoving");

    void Start()
    {
        Debug.Log("--- Player222 SCRIPT IS AWAKE! ---"); // 添加这行

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector2 moveInput = playerMovement.MoveInput;


        bool isCurrentlyMoving = moveInput.sqrMagnitude > 0;

        // 将布尔值传递给Animator的 "isMoving" 参数
        animator.SetBool(isMovingHash, isCurrentlyMoving);

        float horizontalInput = moveInput.x;

        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = false;
        }

        Debug.Log("isCurrentlyMoving: " + isCurrentlyMoving + " | Input: " + moveInput);

        animator.SetBool(isMovingHash, isCurrentlyMoving);


    }
}