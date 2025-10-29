using UnityEngine;
using TMPro; // ���� TextMeshPro �����ռ�
using Assets.C_.player.player;

public class ExperienceBarUI : MonoBehaviour
{
    [Header("�������")]
    [SerializeField] private UnityEngine.UI.Slider experienceSlider;
    [SerializeField] private TextMeshProUGUI experienceText;

    [Header("��������")]
    [Tooltip("��������һ��������ܾ���ֵ")]
    public int experienceToLevelUp = 100; // ��ֵ

    private PlayerState _playerState;

    void Start()
    {
        _playerState = PlayerState.Instance;

        if (experienceSlider == null)
        {
            experienceSlider = GetComponentInChildren<UnityEngine.UI.Slider>();
        }
        if (experienceText == null)
        {
            experienceText = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (_playerState != null)
        {
            UpdateDisplay();
        }
        else
        {
            UnityEngine.Debug.LogError("ExperienceBarUI: �޷��ҵ� PlayerState.Instance��");
        }
    }

    void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_playerState != null && experienceSlider != null && experienceText != null)
        {
            // ����Slider
            experienceSlider.maxValue = experienceToLevelUp;
            experienceSlider.value = _playerState.Experience;

            // �����ı�
            experienceText.text = $"{_playerState.Experience} / {experienceToLevelUp}";
        }
    }
}