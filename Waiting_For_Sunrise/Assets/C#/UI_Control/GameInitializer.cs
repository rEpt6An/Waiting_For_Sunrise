using UnityEngine;
using System.Collections.Generic;
using Assets.C_.common;
using Assets.C_.shop;
using Assets.C_.player;

public class GameInitializer : MonoBehaviour
{
    private static bool isInitialized = false;

    [Header("资源路径检测")]
    [SerializeField] private string itemsJsonPath = "json/Item";
    [SerializeField] private string iconsFolderPath = "item";

    void Awake()
    {
        if (isInitialized)
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("<color=cyan>--- Manual Backend Initialization Started ---</color>");

        // --- 新增：资源导出可用性自检 ---
        CheckResourcesAvailability();

        try
        {
            RegisterCenter.RegisterAll();
            isInitialized = true;
            DontDestroyOnLoad(gameObject);
            Debug.Log("<color=green>--- Backend Initialization Completed Successfully ---</color>");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"<color=red>❌ RegisterCenter 执行崩溃:</color> {e.Message}");
            Debug.LogError(e.StackTrace);
        }
    }

    /// <summary>
    /// 在注册前检查 Resources 下的文件是否存在
    /// </summary>
    private void CheckResourcesAvailability()
    {
        Debug.Log("开始资源就绪性预检...");

        // 1. 检查 JSON 
        TextAsset itemJson = Resources.Load<TextAsset>(itemsJsonPath);
        if (itemJson != null)
        {
            Debug.Log($"✅ JSON 预检成功: 找到 {itemsJsonPath}, 内容长度: {itemJson.text.Length}");
        }
        else
        {
            Debug.LogError($"❌ JSON 预检失败: 无法在 Resources/{itemsJsonPath} 找到文件！请确认后缀是 .json 且在 Resources 文件夹内。");
        }

        // 2. 检查第一个图标 (item/0) 作为代表
        string firstIconPath = iconsFolderPath + "/0";
        Texture2D firstIcon = Resources.Load<Texture2D>(firstIconPath);
        if (firstIcon != null)
        {
            Debug.Log($"✅ 图标预检成功: 找到 {firstIconPath}, 尺寸: {firstIcon.width}x{firstIcon.height}");
        }
        else
        {
            Debug.LogError($"❌ 图标预检失败: 无法在 Resources/{firstIconPath} 找到图标！请确认路径正确且不含扩展名。");
        }
    }
}