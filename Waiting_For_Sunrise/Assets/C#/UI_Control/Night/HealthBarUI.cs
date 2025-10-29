using UnityEngine;
using TMPro; // ���� TextMeshPro �����ռ�
using Assets.C_.player.player;

public class HealthBarUI : MonoBehaviour
{
    [Header("�������")]
    [Tooltip("Ѫ����Slider���")]
    [SerializeField] private UnityEngine.UI.Slider healthSlider;

    [Tooltip("��ʾѪ����ֵ��TextMeshPro�ı����")]
    [SerializeField] private TextMeshProUGUI healthText;

    private PlayerState _playerState;

    void Start()
    {
        _playerState = PlayerState.Instance;

        // ���û����Inspector���ֶ���ק���ã������Զ�����
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<UnityEngine.UI.Slider>();
        }
        if (healthText == null)
        {
            healthText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (_playerState != null)
        {
            // ��ʼ��UI
            UpdateDisplay();
        }
        else
        {
            UnityEngine.Debug.LogError("HealthBarUI: �޷��ҵ� PlayerState.Instance��");
        }
    }

    void Update()
    {
        // ʵʱˢ��UI
        UpdateDisplay();
    }

    /// <summary>
    /// ͳһ����Ѫ��Slider���ı��ķ���
    /// </summary>
    private void UpdateDisplay()
    {
        if (_playerState != null && healthSlider != null && healthText != null)
        {
            // ����Slider
            healthSlider.maxValue = _playerState.MaxHP;
            healthSlider.value = _playerState.Blood;

            // �����ı�
            // ʹ�� $"" ��ʽ���ַ���������ֱ��
            healthText.text = $"{_playerState.Blood} / {_playerState.MaxHP}";
        }
    }
}