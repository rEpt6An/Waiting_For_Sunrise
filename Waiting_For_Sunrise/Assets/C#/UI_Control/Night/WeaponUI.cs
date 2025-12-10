// WeaponUI.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    [Header("主显示区域 (当前武器)")]
    [Tooltip("显示当前装备武器的图片")]
    [SerializeField] private Image currentWeaponImage;
    [Tooltip("显示当前弹药数量的文本组件")]
    [SerializeField] private TextMeshProUGUI currentAmmoText;

    [Header("武器槽位 UI (武器库)")]
    [Tooltip("5个槽位图片，用于显示武器库中的武器")]
    [SerializeField] private Image[] weaponSlots = new Image[5];

    [Tooltip("5个高亮边框/背景，用于指示当前选中的武器")]
    [SerializeField] private Image[] selectionHighlights = new Image[5];

    [Tooltip("没有武器时显示的默认图片")]
    [SerializeField] private Sprite noWeaponSprite;

    // 运行时引用
    private WeaponAnimator currentAnimator;
    private WeaponData currentData;
    private List<WeaponData> weaponArsenal;

    private const int ATTACK_TYPE_MELEE = 0;
    private const int MAX_SLOTS = 5;

    void Start()
    {
        // 确保所有高亮在开始时都是关闭的
        foreach (var highlight in selectionHighlights)
        {
            if (highlight != null)
            {
                highlight.enabled = false;
            }
        }
        // 初始化时，确保主显示区域也清空
        SetNewWeapon(null, null);
    }

    /// <summary>
    /// 初始化武器库显示和当前选中状态。由 PlayerCharacter.Start() 或类似管理器调用。
    /// </summary>
    public void SetupWeaponSlots(List<WeaponData> arsenal, int initialIndex)
    {
        weaponArsenal = arsenal;

        for (int i = 0; i < MAX_SLOTS; i++)
        {
            if (weaponSlots.Length > i && weaponSlots[i] != null)
            {
                if (i < weaponArsenal.Count && weaponArsenal[i] != null)
                {
                    weaponSlots[i].sprite = weaponArsenal[i].image;
                    weaponSlots[i].enabled = true;
                }
                else
                {
                    weaponSlots[i].sprite = noWeaponSprite;
                    weaponSlots[i].enabled = true; // 即使没武器，也显示默认图标
                }
            }
        }

        UpdateCurrentSelection(initialIndex);
    }

    /// <summary>
    /// 由 PlayerCharacter 切换武器时调用，更新主显示区域的引用
    /// </summary>
    public void SetNewWeapon(WeaponAnimator newAnimator, WeaponData newData)
    {
        currentAnimator = newAnimator;
        currentData = newData;

        // 更新主武器图标的显示
        if (currentWeaponImage != null)
        {
            if (currentData != null)
            {
                currentWeaponImage.sprite = currentData.image;
                currentWeaponImage.enabled = true;
            }
            else
            {
                // 如果没有武器，则禁用主武器图标
                currentWeaponImage.enabled = false;
            }
        }
    }

    /// <summary>
    /// 仅更新武器槽位的选中状态 (高亮)
    /// </summary>
    public void UpdateCurrentSelection(int newIndex)
    {
        for (int i = 0; i < MAX_SLOTS; i++)
        {
            if (selectionHighlights.Length > i && selectionHighlights[i] != null)
            {
                if (i == newIndex)
                {
                    // 如果是当前选中的槽位，就高亮
                    selectionHighlights[i].enabled = true;
                    selectionHighlights[i].color = Color.yellow;
                }
                else
                {
                    // 【已修正】如果不是当前选中的槽位，就取消高亮
                    selectionHighlights[i].enabled = false;
                }
            }
        }
    }

    void Update()
    {
        // 确保文本组件存在，否则后续操作无意义
        if (currentAmmoText == null) return;

        // --- 【已修正】---
        // 1. 首先检查对当前武器的引用是否存在
        if (currentAnimator == null || currentData == null)
        {
            // 如果没有武器，显示 "N/A" 并清除主武器图标
            currentAmmoText.text = "N/A";
            if (currentWeaponImage != null)
            {
                currentWeaponImage.enabled = false;
            }
            return; // 提前退出，不再执行后续代码
        }

        // --- 既然引用有效，现在可以安全地获取数据了 ---

        // 确保主显示区可见
        if (currentWeaponImage != null && !currentWeaponImage.enabled)
        {
            currentWeaponImage.enabled = true;
        }

        // --- 更新弹药文本 ---
        if (currentData.attackType == ATTACK_TYPE_MELEE)
        {
            // 近战武器显示无限符号
            currentAmmoText.text = "∞";
        }
        else // 远程武器
        {
            int currentClip = currentAnimator.GetCurrentClip();
            int maxClip = currentData.clip;
            string ammoDisplay = $"{currentClip}/{maxClip}";

            if (currentAnimator.IsReloading())
            {
                // 装弹时显示 LOADING 提示
                currentAmmoText.text = $"{ammoDisplay} \n(RELOADING...)";
            }
            else
            {
                currentAmmoText.text = ammoDisplay;
            }
        }
    }
}