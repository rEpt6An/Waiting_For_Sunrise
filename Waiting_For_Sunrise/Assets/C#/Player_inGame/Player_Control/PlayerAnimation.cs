using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))] // ȷ��PlayerMovement�ű�����
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    // ʹ�� Animator.StringToHash ����������ܣ�������Update�з��������ַ����Ƚ�
    private readonly int isMovingHash = Animator.StringToHash("isMoving");

    void Start()
    {
        Debug.Log("--- Player222 SCRIPT IS AWAKE! ---"); // �������

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector2 moveInput = playerMovement.MoveInput;


        bool isCurrentlyMoving = moveInput.sqrMagnitude > 0;

        // ������ֵ���ݸ�Animator�� "isMoving" ����
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