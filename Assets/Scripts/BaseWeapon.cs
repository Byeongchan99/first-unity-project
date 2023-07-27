using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseWeapon : MonoBehaviour
{
    // 무기의 현재 콤보 카운트
    public int ComboCount { get; set; }

    public string WeaponName { get { return weaponName; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }

    [Header("무기 정보")]
    [SerializeField] protected string weaponName;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;

    public void SetWeaponData(string weaponName, float attackDamage, float attackSpeed, float attackRange)
    {
        this.weaponName = weaponName;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
    }

    public abstract void Attack(BaseState state);
    public abstract void ChargingAttack(BaseState state);
    public abstract void Skill(BaseState state);
}
