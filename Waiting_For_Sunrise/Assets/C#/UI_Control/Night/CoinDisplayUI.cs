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

        // --- �����޸ģ�����ʹ�� Instance�����ǲ��Ҷ��� ---
        // �ڳ����в���Ψһ�� PlayerCharacter ʵ��
        PlayerCharacter playerCharacter = FindObjectOfType<PlayerCharacter>();

        // ����Ƿ�ɹ��ҵ������
        if (playerCharacter != null)
        {
            // ���ҵ������ʵ���л�ȡ PlayerAsset ������
            _playerAsset = playerCharacter.PlayerAsset;
        }
        else
        {
            UnityEngine.Debug.LogError("CoinDisplayUI: �������Ҳ��� PlayerCharacter ʵ����");
        }
    }

    void Update()
    {
        // ʵʱˢ�½����ʾ
        if (_playerAsset != null)
        {
            // ����ֻ������ʾ����ý�ҵ��߼�Ӧ�ڱ𴦵���
            // ���磺playerCharacter.PlayerAsset.ChangeMoney(10);
            coinText.text = $"{_playerAsset.Money}";
        }
    }
}