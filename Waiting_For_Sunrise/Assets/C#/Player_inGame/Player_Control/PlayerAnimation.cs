using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;

    // ������ϣֵ
    private readonly int moveXHash = Animator.StringToHash("moveX");
    private readonly int moveYHash = Animator.StringToHash("moveY");
    private readonly int isMovingHash = Animator.StringToHash("isMoving");
    private readonly int lastMoveXHash = Animator.StringToHash("lastMoveX");
    private readonly int lastMoveYHash = Animator.StringToHash("lastMoveY");
    private readonly int rollTriggerHash = Animator.StringToHash("Roll"); // ��������������


    // ����һ���������洢Roll״̬�Ĺ�ϣֵ
    private int rollStateHash = Animator.StringToHash("Roll State");

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // ������ڷ�������ֹͣ���������ƶ�/վ����������
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

        // �����鷭ת
        if (moveInput.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    // ������������ PlayerMovement �ű�����
    public void TriggerRollAnimation()
    {
        // ��ȡ��ǰ����״̬��Ϣ
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ֻ�е���ǰ�������� "Roll State" ʱ��������������
        if (!stateInfo.IsName("Roll State"))
        {
            animator.SetTrigger(rollTriggerHash);
        }
    }
}