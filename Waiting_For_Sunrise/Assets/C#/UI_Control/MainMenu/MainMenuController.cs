using UnityEngine;
using UnityEngine.SceneManagement; // 必须引用

public class MainMenuController : MonoBehaviour
{
    // 战斗场景的名称，方便在 Inspector 中配置
    [Header("场景名称")]
    [Tooltip("游戏战斗场景的名称")]
    [SerializeField] private string combatSceneName = "Night";

    // 假设您的 SceneSwitcher 挂载在同一个 GameObject 上
    private SceneSwitcher sceneSwitcher;

    void Awake()
    {
        // 尝试获取 SceneSwitcher 组件，如果它与菜单控制器挂载在同一个对象上
        sceneSwitcher = GetComponent<SceneSwitcher>();
        if (sceneSwitcher == null)
        {
            Debug.LogError("MainMenuController: ❌ 找不到 SceneSwitcher 组件! 请确保它已挂载。");
        }
    }

    // --- 按钮点击事件 ---

    // 1. 开始游戏按钮
    public void OnStartGameClicked()
    {
        if (sceneSwitcher != null)
        {
            Debug.Log("MainMenu: 🚀 开始游戏，切换到战斗场景...");
            // 使用 SceneSwitcher 切换到战斗场景
            sceneSwitcher.SwitchScene(combatSceneName);
        }
    }

    // 2. 设置按钮 (占位符)
    public void OnSettingsClicked()
    {
        Debug.Log("MainMenu: ⚙️ 打开设置面板 (TODO)");
    }

    // 3. 帮助按钮 (占位符)
    public void OnHelpClicked()
    {
        Debug.Log("MainMenu: ❓ 打开帮助/教程 (TODO)");
    }

    // 4. 退出按钮
    public void OnQuitGameClicked()
    {
        Debug.Log("MainMenu: 👋 退出游戏...");
        Application.Quit();
        
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
    }
}