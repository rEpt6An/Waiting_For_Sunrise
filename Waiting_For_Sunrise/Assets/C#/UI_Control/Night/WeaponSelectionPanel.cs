// 📜 WeaponSelectionPanel.cs (修正后的完整代码)

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.C_.player.bag;
using TMPro;

[System.Serializable]
public class WeaponSelectionSlot
{
    // ... (WeaponSelectionSlot 类保持不变)
    [Tooltip("显示武器图标的图片组件")]
    public Image icon;
    [Tooltip("显示武器名称的文本组件")]
    public TextMeshProUGUI weaponNameText;
    public Button Button;

    [HideInInspector] public WeaponData weaponData;

    public void SetWeapon(WeaponData data, int slotIndex)
    {
        weaponData = data;

        if (weaponNameText != null)
        {
            weaponNameText.text = data.weaponName;
        }

        if (icon != null)
        {
            icon.sprite = data.image;
            icon.enabled = data.image != null;
        }
    }
}


public class WeaponSelectionPanel : MonoBehaviour
{
    [Header("UI 按钮引用")]
    [SerializeField] private WeaponSelectionSlot[] selectionSlots = new WeaponSelectionSlot[3];

    private List<WeaponData> availablePool;
    private List<WeaponData> playerInventory; // 玩家现有的武器库引用

    // ⭐️ 核心修正：移除 Start() 中的 FindObjectOfType，改为在 SetupSelection 时获取
    private PlayerCharacter player;

    void Start()
    {
        // 自动隐藏，等待 TriggerWeaponSelection() 激活
        gameObject.SetActive(false);
    }

    // ⭐️ 核心修正：在 SetupSelection 被调用时获取 PlayerCharacter
    public void SetupSelection(List<WeaponData> upgradePool, List<WeaponData> playerWeapons)
    {
        // 确保 player 引用有效
        if (player == null)
        {
            player = FindObjectOfType<PlayerCharacter>();
            if (player == null)
            {
                Debug.LogError("WeaponSelectionPanel: 找不到 PlayerCharacter 实例！无法设置升级面板。");
                ResumeGame();
                return;
            }
        }

        if (upgradePool == null || upgradePool.Count < 3)
        {
            Debug.LogWarning("升级武器库不足 3 个，无法进行选择！");
            ResumeGame();
            return;
        }

        availablePool = upgradePool;
        playerInventory = playerWeapons;

        // 1. 随机选择 3 个不重复的武器 (Fisher-Yates 洗牌算法)
        List<WeaponData> shuffledPool = new List<WeaponData>(availablePool);

        // 确保不会抽取到玩家已拥有的武器 (保留)
        shuffledPool.RemoveAll(w => playerInventory.Contains(w));

        if (shuffledPool.Count < 3)
        {
            Debug.LogWarning("升级武器库中可供选择的新武器不足 3 个。");
        }

        // 随机洗牌
        for (int i = 0; i < shuffledPool.Count; i++)
        {
            WeaponData temp = shuffledPool[i];
            int randomIndex = Random.Range(i, shuffledPool.Count);
            shuffledPool[i] = shuffledPool[randomIndex];
            shuffledPool[randomIndex] = temp;
        }

        // 2. 将前三个武器显示在 UI 上
        for (int i = 0; i < selectionSlots.Length; i++)
        {
            if (i < shuffledPool.Count)
            {
                WeaponData selectedWeapon = shuffledPool[i];
                selectionSlots[i].SetWeapon(selectedWeapon, i);
                selectionSlots[i].Button.gameObject.SetActive(true);

                selectionSlots[i].Button.onClick.RemoveAllListeners();
                // 使用匿名函数确保传入正确的武器数据
                selectionSlots[i].Button.onClick.AddListener(() => SelectWeapon(selectedWeapon));
            }
            else
            {
                // 如果武器库不足，隐藏多余的槽位
                selectionSlots[i].Button.gameObject.SetActive(false);
            }
        }
    }

    // 当玩家点击按钮时调用
    public void SelectWeapon(WeaponData weaponToGain)
    {
        // 确保 player 引用有效
        if (player == null)
        {
            Debug.LogError("PlayerCharacter 引用丢失，无法选择武器！");
            ResumeGame();
            return;
        }

        // 检查玩家是否已有该武器
        if (!playerInventory.Contains(weaponToGain))
        {
            // 确保槽位未满 (MAX_WEAPON_SLOTS = 5)
            if (playerInventory.Count < 5)
            {
                // 1. 将新武器添加到玩家的库存
                playerInventory.Add(weaponToGain);

                // 2. 通知 WeaponUI 重新渲染整个武器库
                if (player.weaponUI != null)
                {
                    player.weaponUI.SetupWeaponSlots(playerInventory, player.currentWeaponIndex);
                }

                // 3. 自动切换到新获得的武器
                player.SwitchWeapon(playerInventory.Count - 1);

                Debug.Log($"玩家选择了 {weaponToGain.weaponName}！");
            }
            else
            {
                Debug.Log($"武器槽位已满，获得替代奖励 (金币)。");
                player.GainCoins(50);
            }
        }
        else
        {
            // 如果玩家已经拥有，给予替代奖励
            Debug.Log($"玩家已拥有 {weaponToGain.weaponName}，获得 50 金币替代奖励。");
            player.GainCoins(50);
        }

        ResumeGame();
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // 恢复游戏时间
        gameObject.SetActive(false); // 隐藏面板
    }
}