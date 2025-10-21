using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("����Ŀ��")]
    [Tooltip("���Ҫ�����Ŀ�� (ͨ�������)")]
    [SerializeField] private Transform target;

    [Header("�������")]
    [Tooltip("��������ƽ���ȡ�ֵԽСԽƽ������ӦԽ����")]//���� 0.05 - 0.5 ֮��
    [Range(0.01f, 1f)]
    [SerializeField] private float smoothSpeed = 0.125f;

    [Tooltip("��������ż���ֵԽС����ҰԽ��(�Ŵ�)��ֵԽ����ҰԽԶ(��С)��Ĭ��5�ܳ��á�")]
    [SerializeField] private float cameraZoom = 5f;

    [Tooltip("��������Ŀ��Ĺ̶�ƫ����������2D���ӽǣ���Ҫ����Z��Ϊ���� (��-10) ���ɡ�")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);

    private Camera mainCamera;

    void Awake()
    {
        // ��ȡ���ӵ�����Ϸ�����ϵ�Camera���
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("CameraFollow�ű���Ҫ���ӵ�һ������Camera�������Ϸ�����ϣ�");
            this.enabled = false;
            return;
        }

        // ����Ϸ��ʼʱ��Ӧ��һ�������趨������ֵ
        ApplyZoom();
    }

    // LateUpdate������Update()����֮��ִ�С�
    // ���������������λ�ã�����ȷ������ڱ�֡��λ���Ѿ�������ϣ��������������
    void LateUpdate()
    {
        // ���û������Ŀ�꣬��ִ���κβ���
        if (target == null)
        {
            Debug.LogWarning("���û�����ø���Ŀ�꣡");
            return;
        }

        // 1. �������������λ�� = Ŀ��λ�� + ƫ����
        Vector3 desiredPosition = target.position + offset;

        // 2. ʹ�����Բ�ֵ(Lerp)ƽ�����ƶ����
        // ����������׷�ϡ�Ŀ�꣬������˲������ȥ���Ӷ�����ƽ��Ч��
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 3. ���������λ��
        transform.position = smoothedPosition;
    }

    // Ӧ���趨������ֵ�������Orthographic Size
    private void ApplyZoom()
    {
        if (mainCamera != null && mainCamera.orthographic)
        {
            mainCamera.orthographicSize = cameraZoom;
        }
    }

    // ��������ڱ༭�����޸�ֵʱ�����ã�����������ʵʱԤ������Ч��
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