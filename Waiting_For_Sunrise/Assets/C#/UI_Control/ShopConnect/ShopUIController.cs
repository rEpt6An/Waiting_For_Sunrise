// ShopUIController.cs (固定4个栏位版)
using UnityEngine;
using System.Collections.Generic;
using Assets.C_.shop;
using Assets.C_.player.player;
using Assets.C_.common;
using Assets.C_.common.common;

public class ShopUIController : MonoBehaviour
{
    [Header("UI 引用")]
    [Tooltip("请将4个固定的商品卡片对象拖拽到这里")]
    [SerializeField] private List<ShopSlotUI> shopSlots; // 【核心修改】

    [Header("功能按钮 (可选)")]
    [SerializeField] private UnityEngine.UI.Button refreshButton;

    private IShop _shop;
    private PlayerCharacter _playerCharacter;

    void Start()
    {
        _shop = Shop.GetInstance();
        _playerCharacter = FindObjectOfType<PlayerCharacter>();

        if (_playerCharacter == null)
        {
            UnityEngine.Debug.LogError("商店场景中找不到PlayerCharacter!");
            return;
        }

        // 【核心修改】检查新的引用列表
        if (shopSlots == null || shopSlots.Count == 0)
        {
            UnityEngine.Debug.LogError("ShopUIController 错误: 'Shop Slots' 列表未在Inspector中设置！");
            return;
        }

        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(RefreshShop);
        }

        PopulateShop();
    }

    private void PopulateShop()
    {
        GoodsDto goodsForSale = _shop.GetGoodsForSale();
        if (goodsForSale == null)
        {
            UnityEngine.Debug.LogError("ShopUIController: _shop.GetGoodsForSale() 返回了 null!");
            return;
        }

        // 【核心修改】不再 Instantiate，而是遍历固定的列表
        for (int i = 0; i < shopSlots.Count; i++)
        {
            ShopSlotUI currentSlot = shopSlots[i];

            // 检查后端数据是否存在对应的商品
            if (i < goodsForSale.GoodIds.Count)
            {
                int itemId = goodsForSale.GoodIds[i];

                if (itemId != -1)
                {
                    Item item = ItemManager.Instance.Get(itemId);
                    if (item != null)
                    {
                        // 数据有效，激活UI对象并填充数据
                        currentSlot.gameObject.SetActive(true);
                        currentSlot.Setup(item, i, this);
                        continue; // 继续下一个循环
                    }
                }
            }

            // 如果后端数据不足，或者商品ID无效，则隐藏这个UI栏位
            currentSlot.gameObject.SetActive(false);
        }
    }

    public void RefreshShop()
    {
        var playerState = _playerCharacter.PlayerState;
        if (playerState == null) return;

        int luck = playerState.Lucky;
        int day = 1;
        if (GameManager.Instance != null) day = GameManager.Instance.Day;

        _shop.Flush(new GoodsGetConfig(luck, day));
        PopulateShop();
    }

    public void AttemptToBuyItem(int slotIndex)
    {
        IPlayerAsset playerAsset = _playerCharacter.PlayerAsset;
        if (playerAsset == null) return;

        BuyRequest request = new BuyRequest(slotIndex);
        Re buyResult = _shop.Buy(playerAsset, request);

        if (buyResult.IsSuccess())
        {
            UnityEngine.Debug.Log("购买成功！");
            PopulateShop();
        }
        else
        {
            UnityEngine.Debug.LogWarning("购买失败: " + buyResult.Message);
        }
    }
}