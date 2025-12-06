using UnityEngine;
using TMPro;
using Assets.C_.common;

public class ShopSlotUI : MonoBehaviour
{
    [Header("UI 组件引用")]
    [SerializeField] private UnityEngine.UI.Image itemIcon;
    [SerializeField] private UnityEngine.UI.Image rarityBackground; // 拖拽你的稀有度背景Image到这里
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private UnityEngine.UI.Button buyButton;
    [SerializeField] private UnityEngine.UI.Button lockButton;
    [SerializeField] private GameObject soldOverlay;

    private int _slotIndex;
    private ShopUIController _shopController;
    private bool _isLocked = false;

    void Awake()
    {
        if (buyButton != null) buyButton.onClick.AddListener(OnBuyButtonClicked);
        if (lockButton != null) lockButton.onClick.AddListener(OnLockButtonClicked);
    }

    public void Setup(Item item, int slotIndex, ShopUIController shopController, bool isLocked)
    {
        this._slotIndex = slotIndex;
        this._shopController = shopController;
        this._isLocked = isLocked;

        // --- 1. 设置稀有度颜色 (核心修复) ---
        if (rarityBackground != null)
        {
            // 确保物体是激活的，否则改了颜色也看不见
            rarityBackground.gameObject.SetActive(true);

            // 获取颜色并赋值
            Color rarityColor = GetColorByRarity(item.Rarity);
            rarityBackground.color = rarityColor;

            // 调试日志：如果你还是看不到颜色，看控制台输出什么
            // UnityEngine.Debug.Log($"物品: {item.Name}, 稀有度: {item.Rarity}, 颜色: {rarityColor}");
        }

        // --- 2. 显示图标 ---
        if (itemIcon != null)
        {
            if (item.Icon != null && item.Icon.Image != null)
            {
                itemIcon.sprite = item.Icon.Image;
                itemIcon.enabled = true;
                // 确保图标是白色的（避免被之前的颜色污染），并设置为不透明
                itemIcon.color = Color.white;
            }
            else
            {
                // 没有图标时隐藏Image，避免显示白色方块
                itemIcon.enabled = false;
            }
        }

        // --- 3. 显示文本 ---
        if (itemNameText != null) itemNameText.text = item.Name;
        if (itemPriceText != null) itemPriceText.text = $"${item.Price}";
        if (itemDescriptionText != null) itemDescriptionText.text = item.Description;

        // --- 4. 更新按钮状态 ---
        UpdateLockButtonVisual();

        if (soldOverlay != null) soldOverlay.SetActive(false);
        if (buyButton != null) buyButton.interactable = true;
        if (lockButton != null) lockButton.interactable = true;
    }

    private void OnBuyButtonClicked()
    {
        if (_shopController != null)
        {
            bool success = _shopController.AttemptToBuyItem(_slotIndex);
            if (success)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnLockButtonClicked()
    {
        _isLocked = !_isLocked;
        if (_shopController != null) _shopController.UpdateLockState(_slotIndex, _isLocked);
        UpdateLockButtonVisual();
    }

    private void UpdateLockButtonVisual()
    {
        if (lockButton != null)
        {
            var colors = lockButton.colors;
            colors.normalColor = _isLocked ? Color.yellow : Color.white;
            lockButton.colors = colors;
        }
    }

    // --- 颜色配置逻辑 ---
    private Color GetColorByRarity(Rarity rarity)
    {
        // 这里的 Rarity 必须与你 Assets.C_.common 中的定义一致
        switch (rarity)
        {
            // 注意：你的枚举可能是 Write (笔误) 或者 White (白色)。请根据实际情况调整。
            case Rarity.Write:
                return new Color(0.7f, 0.7f, 0.7f, 1f); // 灰色/白色 (Common)

            case Rarity.Green:
                return new Color(0f, 1f, 0f, 1f);       // 绿色 (Uncommon)

            case Rarity.Blue:
                return new Color(0f, 0.5f, 1f, 1f);     // 蓝色 (Rare)

            case Rarity.Purple:
                return new Color(0.6f, 0f, 0.8f, 1f);   // 紫色 (Epic)

            case Rarity.Red:
                return new Color(1f, 0f, 0f, 1f);       // 红色 (Legendary)

            default:
                // 默认返回半透明黑色，方便发现错误
                return new Color(0f, 0f, 0f, 0.5f);
        }
    }
}