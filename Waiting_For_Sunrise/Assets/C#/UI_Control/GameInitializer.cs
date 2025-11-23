// GameInitializer.cs (最终解决依赖顺序版)
using UnityEngine;
using System.Collections.Generic;
using Assets.C_.common;
using Assets.C_.shop;
using Assets.C_.player;

public class GameInitializer : MonoBehaviour
{
    private static bool isInitialized = false;

    [Header("资源路径")]
    [SerializeField] private string itemsJsonPath = "json/Item"; // Assets/Resources/json/Item.json
    [SerializeField] private string iconsFolderPath = "Icons"; // Assets/Resources/Icons/

    void Awake()
    {
        if (isInitialized)
        {
            Destroy(gameObject);
            return;
        }

        try
        {
            UnityEngine.Debug.Log("--- Manual Backend Initialization Started ---");

            // --- 1. 【关键】首先初始化 IconManager ---
            var iconManager = IconManager.GetInstance();
            if (iconManager == null) throw new System.Exception("Failed to get IconManager instance.");

            // IconManager 的 Load() 需要 List<FileResource>
            // 我们暂时没有真实的图标文件，所以传一个空列表。
            // 这很重要，即使是空的，也要调用Load()，以防它内部有其他初始化逻辑。
            iconManager.Load(new List<FileResource>());
            UnityEngine.Debug.Log("IconManager Initialized (with empty data).");


            // --- 2. 然后初始化 ItemManager，因为它依赖 IconManager ---
            var itemManager = ItemManager.GetInstance();
            if (itemManager == null) throw new System.Exception("Failed to get ItemManager instance.");

            TextAsset itemJsonAsset = Resources.Load<TextAsset>(itemsJsonPath);
            if (itemJsonAsset == null)
                throw new System.IO.FileNotFoundException($"Unity资源文件未找到: Assets/Resources/{itemsJsonPath}");

            // 使用我们修改过的FileResource构造函数
            FileResource itemResource = new FileResource(itemJsonAsset.text);

            // 调用Load方法，它内部会调用ItemConverter，而ItemConverter需要IconManager
            // 因为我们第一步已经初始化了IconManager，所以这里现在是安全的。
            itemManager.Load(itemResource);
            UnityEngine.Debug.Log("ItemManager Initialized and data loaded.");


            // --- 3. 初始化其他服务 ---
            Player.GetInstance();
            Shop.GetInstance();
            UnityEngine.Debug.Log("Player and Shop services initialized.");

            isInitialized = true;
            DontDestroyOnLoad(gameObject);

            UnityEngine.Debug.Log("--- Backend Initialization Completed Successfully ---");
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.LogError($"FATAL: Initialization failed: {ex.Message}\n{ex.StackTrace}");
            this.enabled = false;
        }
    }
}