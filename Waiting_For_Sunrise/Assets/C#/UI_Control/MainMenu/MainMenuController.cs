using UnityEngine;
using UnityEngine.SceneManagement; 
public class MainMenuController : MonoBehaviour
{
    [Header("场景名称")]
    [Tooltip("游戏战斗场景的名称")]
    [SerializeField] private string combatSceneName = "Night";

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


    public void OnStartGameClicked()
    {
        if (sceneSwitcher != null)
        {

            sceneSwitcher.SwitchScene(combatSceneName);
        }
    }

    public void OnSettingsClicked()
    {
    }

    public void OnHelpClicked()
    {
    }

    public void OnQuitGameClicked()
    {
        Debug.Log("MainMenu: 👋 退出游戏...");

        // 如果在 Unity 编辑器中运行
#if UNITY_EDITOR
        // 则停止播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}