using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimator))] // ȷ��PlayerAnimator�ű�����
public class PlayerMovement : MonoBehaviour

{

    public static Transform Instance { get; private set; }

    [Header("�ƶ�����")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("��������")]
    [SerializeField] private float rollDistance = 0.01f; // �����Ĺ̶�����
    [SerializeField] private float rollDuration = 0.2f; // ��������ʱ��
    [SerializeField] private float rollCooldown = 0.1f; // ������ȴʱ�� (Roll_cd)

    private Rigidbody2D rb;
    private PlayerAnimator playerAnimator; // �Զ����ű�������
    private Vector2 moveInput;
    private Vector2 lastMoveDirection; // ��¼��һ�ε��ƶ�����

    private bool isRolling = false;
    private float rollCooldownTimer = 0f;

    // �������ԣ��������ű����԰�ȫ�ض�ȡ�����״̬
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
        // 1. ������ȴ��ʱ
        if (rollCooldownTimer > 0)
        {
            rollCooldownTimer -= Time.deltaTime;
        }

        // 2. ������ڷ������򲻽����κ�������
        if (isRolling)
        {
            return;
        }

        // 3. �������ƶ�����
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (moveInput.sqrMagnitude > 0)
        {
            lastMoveDirection = moveInput;
        }

        // 4. ����������
        if (Input.GetKeyDown(KeyCode.Space) && rollCooldownTimer <= 0)
        {
            StartCoroutine(PerformRoll());
        }
    }

    void FixedUpdate()
    {
        // ֻ���ڲ�������ʱ���Ӧ�ó����ƶ�
        if (!isRolling)
        {
            rb.velocity = moveInput * moveSpeed;
        }
    }

    private IEnumerator PerformRoll()
    {
        // --- ������ʼ ---
        isRolling = true;
        rollCooldownTimer = rollCooldown;

        // ��������
        playerAnimator.TriggerRollAnimation();

        // ȷ�������������û���ƶ���������һ�εķ���
        Vector2 rollDirection = lastMoveDirection.sqrMagnitude > 0 ? lastMoveDirection : Vector2.right;

        // --- �������� ---
        float rollSpeed = rollDistance / rollDuration;
        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            rb.velocity = rollDirection * rollSpeed;
            elapsedTime += Time.fixedDeltaTime; // ʹ��FixedUpdate��ʱ�䲽��
            yield return new WaitForFixedUpdate(); // ������ѭ���еȴ�
        }

        // --- �������� ---
        rb.velocity = Vector2.zero; // ��������������ֹͣ
        isRolling = false;
    }
}