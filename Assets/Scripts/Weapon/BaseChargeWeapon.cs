using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseChargeWeapon : MonoBehaviour
{
    // �� ���⸦ �� �� ���� ���� ��ǥ ����
    public WeaponHandleData HandleData { get { return weaponhandleData; } }
    // �� ���⸦ ����� ���� �ִϸ�����
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }

    public string WeaponName { get { return weaponName; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }

    [Header("���� ����"), Tooltip("���⸦ ����� ���� Local Transform ����")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("���� ����")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string weaponName;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
   
    public void SetWeaponData(string weaponName, float attackDamage, float attackSpeed)
    {
        this.weaponName = weaponName;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
    }

    // �⺻ ����
    public abstract void Attack(BaseState state);
    // ���� Ȱ��ȭ ����
    public abstract void BeginAttack();
    // ���� Ȱ��ȭ ����
    public abstract void EndAttack();
    // ���� ����
    public abstract void ChargingAttack(BaseState state);
    // ��ų
    public abstract void Skill(BaseState state);
}
