using UnityEngine;
using TMPro; // ����TextMeshPro
using Assets.C_.player.player; 

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinDisplayUI : MonoBehaviour
{
    private TextMeshProUGUI coinText;
    private PlayerAsset _playerAsset;

    void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();

        // ��PlayerCharacter�ĵ����л�ȡPlayerAsset������
        if (PlayerCharacter.Instance != null)
        {
            _playerAsset = PlayerCharacter.Instance.PlayerAsset;
        }
    }

    void Update()
    {
        // ʵʱˢ�½����ʾ
        if (_playerAsset != null)
        {
            // ��ĺ�� Re ChangeMoney(int amount) �߼��ܰ�
            // ���ǻ���Ҫһ����ý�ҵķ�������ʱ����ô��
            coinText.text = $"{_playerAsset.Money}";
        }
    }
}