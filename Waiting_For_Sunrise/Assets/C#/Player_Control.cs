using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; // �ƶ��ٶ�

    private Rigidbody2D rb;      // Rigidbody2D���������
    private Vector2 moveInput;   // �洢��������

    void Awake()
    {
        // ��ȡRigidbody2D���������
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveInput.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveInput.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveInput.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveInput.x -= 1;
        }


        if (moveInput.sqrMagnitude > 0)  //��ȡ�������ȵ�ƽ��
        {
            moveInput.Normalize();          // ��һ������������ֹб���ƶ��ٶȹ���

        }
    }

    void FixedUpdate()
    {
        //�̶������������Ӧ���ٶ�
        rb.velocity = moveInput * moveSpeed;
    }
}