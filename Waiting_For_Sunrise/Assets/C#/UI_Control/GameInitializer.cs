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

            RegisterCenter.RegisterAll();

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