using UnityEngine;

// ʹ�� [System.Serializable] ��ǵ�����Ա�Unity���л���
// ����ζ�����ǿ�����ʾ��Inspector�У����ҿ�������ת��ΪJSON��

public enum AttackType { Melee, Ranged }
public enum DamageScaleType { Melee, Ranged } // ��ս & Զ��
// �������������˺����ĸ�������Լӳ�

[System.Serializable]
public class PlayerStats
{
    [Header("��������")]
    public float maxHealth = 100f;   
    public float currentHealth = 100f;
    public float moveSpeed = 5f;

    [Header("��������")]
    public float meleeAttack = 0;  // ��ս�˺�����
    public float rangedAttack = 0; // Զ���˺�����
    [Tooltip("�����˺������ձ���, 1.0 = 100%")]
    public float damageMultiplier = 1.0f;
    [Tooltip("�����ٶȱ���, 1.0 = 100%")]
    public float attackSpeedMultiplier = 1.0f;
    [Tooltip("�����ͼ��ܷ�Χ, Ҳ��ʰȡ��Χ")]
    public float areaSize = 1.0f;

    [Header("��������")]
    [Tooltip("���㹫ʽ��x/x+10")]
    [Range(0f, 1f)]
    public float defense = 0f;
    [Tooltip("���ܼ���, 0.1 = 10%. �������߼�����")]
    [Range(0f, 1f)]
    public float miss = 0f;

    [Header("������")]
    public float accuracy = 1.0f; // ������
    [Tooltip("��������, 0.1 = 10%")]
    [Range(0f, 1f)]
    public float critChance = 0.05f;
    [Tooltip("�����˺�����, 2.0 = 200%")]
    public float critMultiplier = 2.0f;
    [Tooltip("ÿ�ι����ظ�1�������ļ���, 0.1 = 10%")]
    [Range(0f, 1f)]
    public float lifestealChance = 0f;
    [Tooltip("ÿ5��ظ�������ֵ")]
    public float healthRegen = 0f;

    [Header("�ɳ�����")]
    public float luck = 1.0f; // ����ֵ, Ӱ������¼�
    [Tooltip("��ȡ����/��ҵı���, 1.0 = 100%")]
    public float harvestMultiplier = 1.0f;

    public float currentExperience = 0f;//���ھ���
}

[System.Serializable]
public class WeaponStats
{
    [Header("������Ϣ")]
    public string weaponName = "����";
    public AttackType attackType = AttackType.Melee;
    [Tooltip("����ĵ���/�����ж�������")]
    public int projectileCount = 1;

    [Header("�˺�����")]
    public float baseDamage = 5f;
    [Tooltip("����������˺�������ĸ����Լӳ�")]
    public DamageScaleType damageScaleType = DamageScaleType.Melee;
    [Tooltip("��������Լӳɵı���")]
    public float scalingMultiplier = 0.5f;

    [Header("��������")]
    [Tooltip("�����Ļ��������ٶ� (ÿ�빥������)")]
    public float baseAttackSpeed = 1f;
    [Tooltip("Զ��������ɢ��Ƕ� / ��ս�����Ĺ�������")]
    public float spreadAngle = 0f;
    [Tooltip("�����Ļ���������Χ")]
    public float baseRange = 1.5f;
    public float knockback = 1f;

    [Header("�������Լӳ�")]
    [Tooltip("�����ṩ�Ķ�����Ѫ����")]
    public float lifestealBonus = 0f;
    [Tooltip("�����ʳ���, 1.1 = ����10%��ǰ������")]
    public float critChanceMultiplier = 1.0f;
    [Tooltip("�����˺�����, 1.1 = ����10%��ǰ�����˺�")]
    public float critDamageMultiplier = 1.0f;

    [Header("Զ������ר��")]
    public int clipSize = 10;
    [Tooltip("��������ʱ�� (��)")]
    public float baseReloadTime = 2f;
}