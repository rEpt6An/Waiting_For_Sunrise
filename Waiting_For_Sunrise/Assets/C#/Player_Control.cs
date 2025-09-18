using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f; // 移动速度

    private Rigidbody2D rb;      // Rigidbody2D组件的引用
    private Vector2 moveInput;   // 存储输入向量

    void Awake()
    {
        // 获取Rigidbody2D组件的引用
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


        if (moveInput.sqrMagnitude > 0)  //获取向量长度的平方
        {
            moveInput.Normalize();          // 归一化输入向量防止斜向移动速度过快

        }
    }

    void FixedUpdate()
    {
        //固定的物理更新中应用速度
        rb.velocity = moveInput * moveSpeed;
    }
}