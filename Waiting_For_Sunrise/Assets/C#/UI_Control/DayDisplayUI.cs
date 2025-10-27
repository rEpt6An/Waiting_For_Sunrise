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

    //调用方法刷新显示
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