using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标")]
    [Tooltip("相机要跟随的目标 (通常是玩家)")]
    [SerializeField] private Transform target;

    [Header("相机设置")]
    [Tooltip("相机跟随的平滑度。值越小越平滑，反应越慢。")]//建议 0.05 - 0.5 之间
    [Range(0.01f, 1f)]
    [SerializeField] private float smoothSpeed = 0.125f;

    [Tooltip("相机的缩放级别。值越小，视野越近(放大)；值越大，视野越远(缩小)。默认5很常用。")]
    [SerializeField] private float cameraZoom = 5f;

    [Tooltip("相机相对于目标的固定偏移量。对于2D俯视角，主要调整Z轴为负数 (如-10) 即可。")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    private Camera mainCamera;

    void Awake()
    {
        // 获取附加到此游戏对象上的Camera组件
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("CameraFollow脚本需要附加到一个带有Camera组件的游戏对象上！");
            this.enabled = false;
            return;
        }

        // 在游戏开始时，应用一次我们设定的缩放值
        ApplyZoom();
    }

    // LateUpdate在所有Update()调用之后执行。
    // 这是相机跟随的最佳位置，可以确保玩家在本帧的位置已经更新完毕，避免相机抖动。
    void LateUpdate()
    {
        // 如果没有设置目标，则不执行任何操作
        if (target == null)
        {
            Debug.LogWarning("相机没有设置跟随目标！");
            return;
        }

        // 1. 计算相机的期望位置 = 目标位置 + 偏移量
        Vector3 desiredPosition = target.position + offset;

        // 2. 使用线性插值(Lerp)平滑地移动相机
        // 这会让相机“追赶”目标，而不是瞬间贴上去，从而产生平滑效果
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 3. 更新相机的位置
        transform.position = smoothedPosition;
    }

    // 应用设定的缩放值到相机的Orthographic Size
    private void ApplyZoom()
    {
        if (mainCamera != null && mainCamera.orthographic)
        {
            mainCamera.orthographicSize = cameraZoom;
        }
    }

    // 这个函数在编辑器中修改值时被调用，可以让我们实时预览缩放效果
#if UNITY_EDITOR
    void OnValidate()
    {
        if (mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
        }
        ApplyZoom();
    }
#endif
}