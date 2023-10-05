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
    public int BulletID { get { return bulletID; } }
    public float DamageCoefficient { get { return damageCoefficient; } }

    [Header("���� ����"), Tooltip("���⸦ ����� ���� Local Transform ����")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("���� ����")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string weaponName;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int bulletID;   // ����ϴ� �Ѿ��� ������ ID
    [SerializeField] protected float damageCoefficient;
   
    public void SetWeaponData(string weaponName, float attackDamage, float attackSpeed, int bulletID, float damageCoefficient)
    {
        this.weaponName = weaponName;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.bulletID = bulletID;
        this.damageCoefficient = damageCoefficient;
    }

    // �⺻ ����
    public abstract void Attack(BaseState state);
    // ���� Ȱ��ȭ ����
    public abstract void BeginAttack();
    // ���� Ȱ��ȭ ����
    public abstract void EndAttack();
    // ���� ��ġ ���콺 ���� ȸ��
    public abstract void RotateWeaponTowardsMouse();
    // �� ��ġ ����
    public abstract void UpdateWeaponAndHandPosition(float chargeTime);
    // ���� ����
    public abstract void ChargingAttack(BaseState state, Vector2 dir, int chargeLevel);
    // ��ų
    public abstract void Skill(BaseState state);
}
