using UnityEngine;
// 假设 Boss 死亡是基于 HP 或类似属性

public class Boss : MonoBehaviour
{


    // 假设您在 Boss 死亡后会调用此方法
    public void OnBossDeath()
    {
        if (GameManager.Instance != null)
        {
            Debug.Log("Boss 已被击败！触发游戏胜利。");
            GameManager.Instance.HandleGameVictory();

            // 延迟销毁或禁用 Boss 对象
            Destroy(gameObject, 0.5f);
        }
    }

}