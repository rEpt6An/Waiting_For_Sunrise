// PlayerCharacter.cs
using UnityEngine;
// �������˴������ڵ������ռ�
using Assets.C_.player.player;
using Assets.C_.player.bag; // ȷ�� PlayerAsset �ܱ���ȷʶ��

// ȷ����Ҷ���������Щ�������
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("��������")]
    [SerializeField] private WeaponData currentWeapon;
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private Transform attackSpawnPoint;

    // --- ����������� ---
    private PlayerState _playerState;
    // --- ���ؼ������� PlayerAsset ���� ---
    public PlayerAsset PlayerAsset { get; private set; }

    // --- �ڲ�״̬ ---
    private float attackCooldownTimer = 0f;

    void Awake()
    {
        // --- ��ʼ��������� ---
        _playerState = PlayerState.Instance;
        PlayerAsset = new PlayerAsset(); // ���� PlayerAsset ��Ψһʵ��
    }

    void Update()
    {
        // ������ȴ��ʱ
        attackCooldownTimer += Time.deltaTime;

        // ������������Ƿ����
        if (currentWeapon == null) return;

        // ���㹥����ȴʱ��
        // ע�⣺��ҹ��� AttackSpeed �ĳ�ʼֵ��ӦΪ0������ᵼ�³��������
        // �����������һ�������������ҹ���С�ڵ���0����ʹ��һ��Ĭ��ֵ
        double playerAttackSpeed = _playerState.AttackSpeed > 0 ? _playerState.AttackSpeed : 1.0;
        float attackCooldown = 1f / (currentWeapon.baseAttackSpeed * (float)playerAttackSpeed);

        // �����������ס���������ȴ
        if (Input.GetMouseButton(0) && attackCooldownTimer >= attackCooldown)
        {
            PerformAttack();
            attackCooldownTimer = 0f; // ������ȴ
        }
    }

    private void PerformAttack()
    {
        // 1. ��ȡ��귽��
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - attackSpawnPoint.position).normalized;

        // 2. ��������ʵ��
        GameObject attackInstance = Instantiate(attackPrefab, attackSpawnPoint.position, Quaternion.identity);

        // 3. ��ת����ʵ��ʹ�䳯�����
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        // 4. ���������˺�
        float correspondingPlayerDamage = 0;
        // ����˺��ӳ�����: 0=��ս, 1=Զ��
        if (currentWeapon.damageScaleType == 0)
        {
            correspondingPlayerDamage = _playerState.MeleeAttack;
        }
        else if (currentWeapon.damageScaleType == 1)
        {
            correspondingPlayerDamage = _playerState.RangedAttack;
        }

        float finalDamage = (currentWeapon.baseDamage + currentWeapon.scalingMultiplier * correspondingPlayerDamage)
                            * (float)_playerState.DamageMultipler;

        // 5. ���˺�ֵ���ݸ������ű�
        WeaponAttack attackScript = attackInstance.GetComponent<WeaponAttack>();
        if (attackScript != null)
        {
            attackScript.Initialize(finalDamage);
        }
    }

    // --- ���״̬�ӿ� ---
    // �����ű����ڿ���ͨ�� FindObjectOfType<PlayerCharacter>() ��������Щ����
    public void TakeDamage(int damageAmount)
    {
        // δ�����������������������ܼ���
        _playerState.changeBlood(-damageAmount);

        if (_playerState.isDie())
        {
            HandleDeath();
        }
    }

    public void GainExperience(int amount)
    {
        _playerState.changeExperience(amount);
    }

    private void HandleDeath()
    {
        UnityEngine.Debug.LogWarning("�����������");
        // �򵥵ؽ�����Ҷ��󣬿��Ի��ɸ����ӵ������߼�
        gameObject.SetActive(false);
    }
}