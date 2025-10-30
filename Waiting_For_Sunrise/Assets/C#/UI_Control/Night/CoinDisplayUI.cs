using UnityEngine;
using TMPro;
using Assets.C_.player.player;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinDisplayUI : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private PlayerAsset _playerAsset;

    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();

        // --- 核心修改：不再使用 Instance，而是查找对象 ---
        // 在场景中查找唯一的 PlayerCharacter 实例
        PlayerCharacter playerCharacter = FindObjectOfType<PlayerCharacter>();

        // 检查是否成功找到了玩家
        if (playerCharacter != null)
        {
            // 从找到的玩家实例中获取 PlayerAsset 的引用
            _playerAsset = playerCharacter.PlayerAsset;
        }
        else
        {
            UnityEngine.Debug.LogError("CoinDisplayUI: 场景中找不到 PlayerCharacter 实例！");
        }
    }

    void Update()
    {
        // 实时刷新金币显示
        if (_playerAsset != null)
        {
            // 这里只负责显示，获得金币的逻辑应在别处调用
            // 例如：playerCharacter.PlayerAsset.ChangeMoney(10);
            coinText.text = $"{_playerAsset.Money}";
        }
    }
}