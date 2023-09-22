using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseWeapon : MonoBehaviour
{
    // ������ ���� �޺� ī��Ʈ
    public int ComboCount { get; set; }
    // �� ���⸦ �� �� ���� ���� ��ǥ ����
    public WeaponHandleData HandleData { get { return weaponhandleData; } }
    // �� ���⸦ ����� ���� �ִϸ�����
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }

    public string WeaponName { get { return weaponName; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }
    public float AdvanceDistance { get { return advanceDistance; } }

    [Header("���� ����"), Tooltip("���⸦ ����� ���� Local Transform ����")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("���� ����")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string weaponName;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;
    // ���� �� ���� �Ÿ�
    [SerializeField] protected float advanceDistance;

    public void SetWeaponData(string weaponName, float attackDamage, float attackSpeed, float attackRange, float advanceDistance)
    {
        this.weaponName = weaponName;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.advanceDistance = advanceDistance;
    }

    // �⺻ ����
    public abstract void Attack(BaseState state);
    // ���� ����
    public abstract void ChargingAttack(BaseState state);
    // ��ų
    public abstract void Skill(BaseState state);
}
