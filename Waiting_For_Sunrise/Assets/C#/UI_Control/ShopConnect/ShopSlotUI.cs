using UnityEngine;
using TMPro;
using Assets.C_.common;

public class ShopSlotUI : MonoBehaviour
{
    [Header("UI 组件引用")]
    [SerializeField] private UnityEngine.UI.Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private UnityEngine.UI.Button buyButton;

    private int _itemId;
    private int _slotIndex;
    private ShopUIController _shopController;

    void Awake()
    {
        if (buyButton != null)
        {
            buyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    public void Setup(Item item, int slotIndex, ShopUIController shopController)
    {
        this._itemId = item.Id;
        this._slotIndex = slotIndex;
        this._shopController = shopController;

        if (itemIcon != null) itemIcon.color = GetColorByRar(item.Rarity);
        if (itemNameText != null) itemNameText.text = item.Name;
        if (itemPriceText != null) itemPriceText.text = $"${item.Price}";
    }

    private void OnBuyButtonClicked()
    {
        if (_shopController != null)
        {
            _shopController.AttemptToBuyItem(_slotIndex);
        }
    }

    private Color GetColorByRar(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Write: return Color.white;
            case Rarity.Green: return Color.green;
            case Rarity.Blue: return Color.blue;
            case Rarity.Purple: return new Color(0.5f, 0, 0.5f);
            case Rarity.Red: return Color.red;
            default: return Color.grey;
        }
    }
}