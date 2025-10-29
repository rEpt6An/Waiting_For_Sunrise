using UnityEngine;
using Assets.C_.player.player;

// ȷ����Ҷ���������Щ�������
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCharacter : MonoBehaviour
{
    // --- ����������� ---
    // ������Ĵ�C#���ݲ㣬�������������ֵ
    private PlayerState _playerState;

    // --- Unity������� ---
    // [Header("UI�������")]
    // public HealthBarUI healthBar; // ʾ����Ѫ��UI�ű�
    // public ExperienceBarUI expBar; // ʾ����������UI�ű�
    // public CoinDisplayUI coinDisplay; // ʾ�������UI�ű�

    // ��̬���������������ű�������EnemyController�����ٷ������
    public static PlayerCharacter Instance { get; private set; }

    public PlayerAsset PlayerAsset { get; private set; } // ��PlayerAsset��Ϊ��������

    void Awake()
    {
        // --- ����ģʽʵ�� ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerState = PlayerState.Instance;
        PlayerAsset = new PlayerAsset(); // �����ﴴ��Ψһʵ��

        // �������������һ�γ�ʼUI����
        UpdateAllUI();
    }

    // --- ���Ĺ��ܷ��� ---

    /// <summary>
    /// ������ܵ��˺�ʱ����
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        // ���������������������ܼ���
        // ʾ���� if (Random.value < _playerState.Dodge) { return; } // ���ܳɹ�
        // int finalDamage = Mathf.Max(1, damageAmount - _playerState.DefensivePower);

        _playerState.changeBlood(-damageAmount); // ֱ�ӵ��ú�˷���
        UnityEngine.Debug.Log($"����ܵ� {damageAmount} �˺�, ʣ��Ѫ��: {_playerState.Blood}");

        // ����Ѫ��UI
        // healthBar.UpdateHealth(_playerState.Blood, _playerState.MaxHP);

        if (_playerState.isDie())
        {
            HandleDeath();
        }
    }

    /// <summary>
    /// ����һ�þ���ʱ����
    /// </summary>
    public void GainExperience(int amount)
    {
        _playerState.changeExperience(amount);
        UnityEngine.Debug.Log($"��� {amount} ����, �ܾ���: {_playerState.Experience}");

        // ���¾���UI
        // expBar.UpdateExperience(_playerState.Experience, requiredExp); // (��Ҫһ�������������辭����߼�)
    }

    private void HandleDeath()
    {
        UnityEngine.Debug.LogWarning("�����������");
        // ��������ƶ��͹�����
        GetComponent<PlayerMovement>().enabled = false;
        // ���������ﴥ��Game Over�߼�
    }

    /// <summary>
    /// һ�����еķ�������������Ϸ��ʼ����Ҫʱ��������UI
    /// </summary>
    public void UpdateAllUI()
    {
        // healthBar.UpdateHealth(_playerState.Blood, _playerState.MaxHP);
        // expBar.UpdateExperience(_playerState.Experience, requiredExp);
        // coinDisplay.UpdateCoins(PlayerAsset.Instance.Money); // ����PlayerAssetҲ�ǵ���
    }

    // ��Update�м�⹥������
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0����������
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        UnityEngine.Debug.Log("��ҷ����˹�����");
        // TODO: ������ʵ�ֹ����߼�
        // 1. ȷ��������Χ (һ��Բ��һ������)
        // 2. ��ⷶΧ�ڵ����е���
        // 3. �Լ�⵽��ÿ�����˵�����TakeDamage����
    }
}