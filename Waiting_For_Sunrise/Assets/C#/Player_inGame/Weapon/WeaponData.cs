using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Data/Weapon Data")]
public class WeaponData : ScriptableObject
{
    // �������ﶨ���κ� enum

    [Header("������ֵ")]
    public string weaponName = "��������";

    [Tooltip("��������: 0=��ս, 1=Զ��")]
    public int attackType = 0;

    [Tooltip("�˺��ӳ�����: 0=�ܽ�ս�ӳ�, 1=��Զ�̼ӳ�")]
    public int damageScaleType = 0;

    public float baseDamage = 10f;
    public float scalingMultiplier = 0.5f;
    public float baseAttackSpeed = 1f;
}