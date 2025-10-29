using UnityEngine;
using TMPro; // 引入TextMeshPro
using Assets.C_.player.player; 

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinDisplayUI : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private PlayerAsset _playerAsset;

    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();

        // 从PlayerCharacter的单例中获取PlayerAsset的引用
        if (PlayerCharacter.Instance != null)
        {
            _playerAsset = PlayerCharacter.Instance.PlayerAsset;
        }
    }

    void Update()
    {
        // 实时刷新金币显示
        if (_playerAsset != null)
        {
            // 你的后端 Re ChangeMoney(int amount) 逻辑很棒
            // 我们还需要一个获得金币的方法，暂时先这么做
            coinText.text = $"{_playerAsset.Money}";
        }
    }
}