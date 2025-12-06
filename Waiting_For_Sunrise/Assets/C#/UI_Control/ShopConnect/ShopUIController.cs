// ShopUIController.cs
using UnityEngine;
using System.Collections.Generic;
using Assets.C_.shop;
using Assets.C_.player.player;
using Assets.C_.common;
using Assets.C_.common.common;

public class ShopUIController : MonoBehaviour
{
    [Header("UI 引用")]
    [SerializeField] private List<ShopSlotUI> shopSlots;
    [SerializeField] private UnityEngine.UI.Button refreshButton;

    [SerializeField] private TMPro.TextMeshProUGUI refreshPriceText; 

    [Header("商店逻辑设置")]
    [SerializeField] private int baseRefreshPrice = 5;
    [SerializeField] private int priceIncreasePerRefresh = 5;

    // --- 内部状态 ---
    private IShop _shop;
    private PlayerCharacter _playerCharacter;
    private List<int> _lockedSlotIndexes = new List<int>();
    private int _currentRefreshPrice;

    void Start()
    {
        _shop = Shop.GetInstance();
        _playerCharacter = FindObjectOfType<PlayerCharacter>();

        if (_playerCharacter == null)
        {
            UnityEngine.Debug.LogError("商店场景中找不到PlayerCharacter!");
            return;
        }

        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(RefreshShop);
        }

        // 新的一天开始，重置刷新价格
        ResetRefreshPrice();

        InitialShopFlush();
        PopulateShop();
    }

    private void InitialShopFlush()
    {
        int luck = _playerCharacter.PlayerState.Lucky;
        int day = GameManager.Instance != null ? GameManager.Instance.Day : 1;
        _shop.Flush(new GoodsGetConfig(luck, day, new List<int>()));
    }

    private void PopulateShop()
    {
        GoodsDto goodsForSale = _shop.GetGoodsForSale();
        if (goodsForSale == null) return;

        for (int i = 0; i < shopSlots.Count; i++)
        {
            ShopSlotUI currentSlot = shopSlots[i];
            if (i < goodsForSale.GoodIds.Count)
            {
                int itemId = goodsForSale.GoodIds[i];
                if (itemId != -1)
                {
                    Item item = ItemManager.Instance.Get(itemId);
                    if (item != null)
                    {
                        currentSlot.gameObject.SetActive(true);
                        bool isSlotLocked = _lockedSlotIndexes.Contains(i);
                        currentSlot.Setup(item, i, this, isSlotLocked);
                        continue;
                    }
                }
            }
            currentSlot.gameObject.SetActive(false);
        }
    }

    public void RefreshShop()
    {
        // 1. 检查玩家金币是否足够
        if (_playerCharacter.PlayerAsset.Money < _currentRefreshPrice)
        {
            UnityEngine.Debug.LogWarning("金币不足，无法刷新商店！");
            // TODO: 在UI上显示提示
            return;
        }

        // 2. 扣除金币
        _playerCharacter.PlayerAsset.ChangeMoney(-_currentRefreshPrice);

        // 3. 增加下一次刷新的价格
        _currentRefreshPrice += priceIncreasePerRefresh;
        UpdateRefreshPriceText();

        // 4. 调用后端刷新逻辑
        int luck = _playerCharacter.PlayerState.Lucky;
        int day = GameManager.Instance != null ? GameManager.Instance.Day : 1;
        _shop.Flush(new GoodsGetConfig(luck, day, _lockedSlotIndexes));

        // 5. 更新UI
        PopulateShop();
    }

    public bool AttemptToBuyItem(int slotIndex)
    {
        Re buyResult = _shop.Buy(_playerCharacter.PlayerAsset, new BuyRequest(slotIndex));

        if (buyResult.IsSuccess())
        {
            UnityEngine.Debug.Log("购买成功！");
            return true;
        }
        else
        {
            UnityEngine.Debug.LogWarning("购买失败: " + buyResult.Message);
            return false;
        }
    }

    public void UpdateLockState(int slotIndex, bool isLocked)
    {
        if (isLocked)
        {
            if (!_lockedSlotIndexes.Contains(slotIndex)) _lockedSlotIndexes.Add(slotIndex);
        }
        else
        {
            if (_lockedSlotIndexes.Contains(slotIndex)) _lockedSlotIndexes.Remove(slotIndex);
        }
    }

    // 新增：重置刷新价格的方法
    public void ResetRefreshPrice()
    {
        _currentRefreshPrice = baseRefreshPrice;
        UpdateRefreshPriceText();
    }

    // 新增：更新刷新价格显示的方法
    private void UpdateRefreshPriceText()
    {
        if (refreshPriceText != null)
        {
            refreshPriceText.text = $"{_currentRefreshPrice}";
        }
    }


}