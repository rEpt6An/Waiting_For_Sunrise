// PlayerBagUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.C_.player.bag;
using Assets.C_.player.player;
using TMPro; // 需要引入TextMeshPro

public class PlayerBagUI : MonoBehaviour
{
    [Header("UI 容器")]
    public RectTransform InventoryContentParent;
    public RectTransform ToolbarParent;

    [Header("UI 预制件")]
    public GameObject ItemSlotPrefab;

    [Header("详情面板")]
    public GameObject ItemDetailPanel; // 拖入你的详情面板
    public TextMeshProUGUI DetailItemName;
    public TextMeshProUGUI DetailItemDescription;
    public Button EquipButton;

    private IPlayerBag _playerBagData;
    private List<ItemSlotUI> _inventorySlots = new List<ItemSlotUI>();
    private ItemSlotUI[] _toolbarSlots;

    private ItemSlotUI _selectedSlot; // 记录当前被选中的槽位

    private const int ToolbarSize = 5; // 你的代码是5，我保持一致

    void Start()
    {
        var playerCharacter = FindObjectOfType<PlayerCharacter>();
        if (playerCharacter != null && playerCharacter.PlayerAsset != null)
        {
            _playerBagData = playerCharacter.PlayerAsset.GetPlayerBag();
        }

        if (_playerBagData == null)
        {
            Debug.LogError("PlayerBagUI: 关键错误！未能获取到 PlayerBagData。");
            gameObject.SetActive(false);
            return;
        }

        InitializeUI();
        ItemDetailPanel.SetActive(false); // 初始时隐藏详情面板
    }

    void Update()
    {
        RefreshUI();
    }

    private void InitializeUI()
    {
        foreach (Transform child in ToolbarParent) Destroy(child.gameObject);

        _toolbarSlots = new ItemSlotUI[ToolbarSize];
        for (int i = 0; i < ToolbarSize; i++)
        {
            GameObject slotObject = Instantiate(ItemSlotPrefab, ToolbarParent);
            ItemSlotUI slotUI = slotObject.GetComponent<ItemSlotUI>();
            slotUI.Initialize(i, true, this);
            _toolbarSlots[i] = slotUI;
        }

        foreach (Transform child in InventoryContentParent) Destroy(child.gameObject);
    }

    public void RefreshUI()
    {
        if (_playerBagData == null) return;
        RefreshInventory();
        RefreshToolbar();
    }

    private void RefreshInventory()
    {
        List<PileOfItem> currentItems = _playerBagData.GetInventory().GetAll();

        while (_inventorySlots.Count < currentItems.Count)
        {
            GameObject slotObject = Instantiate(ItemSlotPrefab, InventoryContentParent);
            ItemSlotUI slotUI = slotObject.GetComponent<ItemSlotUI>();
            slotUI.Initialize(_inventorySlots.Count, false, this);
            _inventorySlots.Add(slotUI);
        }

        for (int i = 0; i < _inventorySlots.Count; i++)
        {
            if (i < currentItems.Count)
            {
                _inventorySlots[i].gameObject.SetActive(true);
                _inventorySlots[i].UpdateSlot(currentItems[i]);
            }
            else
            {
                _inventorySlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void RefreshToolbar()
    {
        PileOfItem[] currentItems = _playerBagData.GetToolbar().GetAll();
        for (int i = 0; i < ToolbarSize; i++)
        {
            PileOfItem item = (i < currentItems.Length) ? currentItems[i] : null;
            _toolbarSlots[i].UpdateSlot(item);
        }
    }

    /// <summary>
    /// 当任何一个槽位被点击时，此方法被调用
    /// </summary>
    public void OnSlotClicked(ItemSlotUI clickedSlot)
    {
        _selectedSlot = null; // 先清空之前的选择

        if (clickedSlot.CurrentItem == null)
        {
            // 如果点击的是空格子，直接隐藏详情面板
            ItemDetailPanel.SetActive(false);
            return;
        }

        // --- 核心修改：显示详情面板，而不是直接移动 ---
        _selectedSlot = clickedSlot; // 记录被选中的槽位

        // 获取物品的详细信息 (你需要确保Item类有Description属性)
        var itemData = Assets.C_.common.ItemManager.Instance.Get(clickedSlot.CurrentItem.ItemId);

        // 更新详情面板的显示内容
        DetailItemName.text = itemData.Name;
        // DetailItemDescription.text = itemData.Description; // 假设Item类有Description

        // 绑定“装备”按钮的点击事件
        EquipButton.onClick.RemoveAllListeners();

        // 只有背包里的物品才能被装备
        if (!clickedSlot.IsToolbar)
        {
            EquipButton.gameObject.SetActive(true); // 显示装备按钮
            EquipButton.onClick.AddListener(OnEquipButtonClicked);
        }
        else
        {
            EquipButton.gameObject.SetActive(false); // 如果是工具栏物品，隐藏装备按钮
        }

        // 显示详情面板
        ItemDetailPanel.SetActive(true);
    }

    /// <summary>
    /// 当详情面板中的“装备”按钮被点击时调用
    /// </summary>
    private void OnEquipButtonClicked()
    {
        if (_selectedSlot == null || _selectedSlot.CurrentItem == null) return;

        int targetIndex = GetFirstEmptyToolbarIndex();
        if (targetIndex != -1)
        {
            Debug.Log($"尝试将物品 {_selectedSlot.CurrentItem.ItemId} 装备到工具栏位 {targetIndex}");
            _playerBagData.MoveToToolbar(_selectedSlot.CurrentItem, targetIndex);

            // 装备成功后，隐藏详情面板
            ItemDetailPanel.SetActive(false);
            _selectedSlot = null;
        }
        else
        {
            Debug.Log("工具栏已满，无法装备！");
            // 可以在这里给玩家一个UI提示
        }
    }
    private int GetFirstEmptyToolbarIndex()
    {
        PileOfItem[] toolbarItems = _playerBagData.GetToolbar().GetAll();
        for (int i = 0; i < ToolbarSize; i++)
        {
            if (i >= toolbarItems.Length || toolbarItems[i] == null || toolbarItems[i].Count == 0)
            {
                return i;
            }
        }
        return -1;
    }
}