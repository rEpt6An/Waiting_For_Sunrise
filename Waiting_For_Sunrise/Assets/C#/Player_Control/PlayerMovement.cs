using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f; 

    private Rigidbody2D rb;      //
    private Vector2 moveInput;   // 存储输入向量



    public Vector2 MoveInput { get { return moveInput; } }    //


    void Start()
    {
        Debug.Log("--- PlayerMovement SCRIPT IS AWAKE! ---"); // 添加这行

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
        rb.velocity = moveInput * moveSpeed;
    }
}