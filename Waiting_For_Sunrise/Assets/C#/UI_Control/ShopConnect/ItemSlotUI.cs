// ItemSlotUI.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.C_.player.bag;
using Assets.C_.common;

public class ItemSlotUI : MonoBehaviour
{
    [Header("UI 引用")]
    public Image IconImage; // 你的代码中已经用了 Image，保持即可
    public TextMeshProUGUI CountText;
    public Button ClickButton;

    public int Index { get; private set; }
    public bool IsToolbar { get; private set; }
    public PileOfItem CurrentItem { get; private set; }

    public void Initialize(int index, bool isToolbar, PlayerBagUI bagUI)
    {
        this.Index = index;
        this.IsToolbar = isToolbar;

        if (bagUI != null && ClickButton != null)
        {
            ClickButton.onClick.RemoveAllListeners();
            ClickButton.onClick.AddListener(() => bagUI.OnSlotClicked(this));
        }
    }

    public void UpdateSlot(PileOfItem item)
    {
        CurrentItem = item;

        if (item != null && item.Count > 0)
        {
            IconImage.enabled = true;

            Icon iconData = IconManager.Instance.Get(item.ItemId);

            // --- 【核心修正】: 使用后端定义的 .Image 属性，而不是 .Sprite ---
            if (iconData != null && iconData.Image != null)
            {
                IconImage.sprite = iconData.Image; // 使用 iconData.Image
                IconImage.color = Color.white;
            }
            else
            {
                IconImage.sprite = null;
                IconImage.color = new Color(1, 0, 1, 0.5f);
            }

            if (item.Count > 1)
            {
                CountText.enabled = true;
                CountText.text = item.Count.ToString();
            }
            else
            {
                CountText.enabled = false;
            }
        }
        else
        {
            IconImage.enabled = false;
            IconImage.sprite = null;
            CountText.enabled = false;
        }
    }
}