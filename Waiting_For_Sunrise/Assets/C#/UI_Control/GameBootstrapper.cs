// GameBootstrapper.cs
using UnityEngine;
// 引入你所有后端管理器的命名空间
using Assets.C_.common;
using Assets.C_.shop;
using Assets.C_.player;

public class GameBootstrapper : MonoBehaviour
{
    // 使用一个静态变量来确保这个引导程序本身也是单例
    public static GameBootstrapper Instance { get; private set; }

    void Awake()
    {
        // --- 单例模式 + 跨场景持久化 ---
        if (Instance != null && Instance != this)
        {
            // 如果场景中已经存在一个引导程序，说明我们是从别的场景返回
            // 这个新的引导程序是多余的，销毁它
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // 【关键】让这个对象在加载新场景时不被销毁
        DontDestroyOnLoad(gameObject);

        // --- 初始化所有后端核心服务 ---
        InitializeBackendServices();
    }

    private void InitializeBackendServices()
    {
        UnityEngine.Debug.Log("--- Initializing All Backend Services ---");

        // 按照依赖顺序，强制初始化所有单例管理器
        // 1. 初始化 ItemManager，因为它被 Shop 依赖
        var itemManager = ItemManager.Instance;
        if (itemManager != null)
        {
            UnityEngine.Debug.Log("ItemManager Initialized Successfully.");
        }
        else
        {
            UnityEngine.Debug.LogError("Failed to initialize ItemManager!");
        }

        // 2. 初始化 Player，因为它可能被其他系统依赖
        var player = Player.GetInstance();
        if (player != null)
        {
            UnityEngine.Debug.Log("Player Initialized Successfully.");
        }

        // 3. 初始化 Shop，它依赖 ItemManager
        var shop = Shop.GetInstance();
        if (shop != null)
        {
            UnityEngine.Debug.Log("Shop Initialized Successfully.");
        }

        // ... 初始化其他任何后端管理器 ...
    }
}