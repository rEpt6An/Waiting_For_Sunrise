using UnityEngine;
using TMPro;
using System.Collections;   
using System.Collections.Generic; 

[RequireComponent(typeof(TextMeshProUGUI))]
public class CountdownTimer : MonoBehaviour
{
    [Header("��ʱ������")]
    [Tooltip("����ʱ����ʱ�����룩")]
    [SerializeField] private float timeDuration = 60f;

    private TextMeshProUGUI timerText;
    private float timer;

    [Header("����ʱ������ת")]
    [Tooltip("�����л�����������������������п����գ�")]
    [SerializeField] private SceneSwitcher sceneSwitcher;//��������

    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();

        // ���������û�й� SceneSwitcher�����Զ���һ��
        if (sceneSwitcher == null) sceneSwitcher = FindObjectOfType<SceneSwitcher>();
    }

    void Start() => ResetTimer();

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            timer = 0;
            UpdateTimerDisplay();
            this.enabled = false;

            if (sceneSwitcher != null)
                sceneSwitcher.SwitchScene("Shop");
            else
                UnityEngine.Debug.LogWarning("CountdownTimer: �Ҳ��� SceneSwitcher���޷���ת Shop��");
        }
    }

    public void ResetTimer()
    {
        timer = timeDuration;
        UpdateTimerDisplay();
        this.enabled = true;
    }

    private void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(timer);
        timerText.text = $"{seconds}";
    }
}