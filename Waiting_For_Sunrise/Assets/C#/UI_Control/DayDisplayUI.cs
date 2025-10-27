using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DayDisplayUI : MonoBehaviour
{
    private TextMeshProUGUI dayText;

    void Start()
    {
        dayText = GetComponent<TextMeshProUGUI>();
        UpdateDayText();
    }

    //���÷���ˢ����ʾ
    public void UpdateDayText()
    {
        if (GameManager.Instance != null)
        {
            dayText.text = GameManager.Instance.GetDayString();
        }
        else
        {
            dayText.text = "Day: N/A";
        }
    }
}