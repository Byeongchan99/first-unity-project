using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseWeapon : MonoBehaviour
{
    // 무기의 현재 콤보 카운트
    public int ComboCount { get; set; }
    // 이 무기를 쥘 때 손의 로컬 좌표 정보
    public WeaponHandleData HandleData { get { return weaponhandleData; } }
    // 이 무기를 사용할 때의 애니메이터
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }

    public string WeaponName { get { return weaponName; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public float AttackRange { get { return attackRange; } }
    public float AdvanceDistance { get { return advanceDistance; } }

    [Header("생성 정보"), Tooltip("무기를 쥐었을 때의 Local Transform 정보")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("무기 정보")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string weaponName;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackRange;
    // 공격 시 전진 거리
    [SerializeField] protected float advanceDistance;

    public void SetWeaponData(string weaponName, float attackDamage, float attackSpeed, float attackRange, float advanceDistance)
    {
        this.weaponName = weaponName;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.attackRange = attackRange;
        this.advanceDistance = advanceDistance;
    }

    // 기본 공격
    public abstract void Attack(BaseState state);
    // 무기 활성화 시작
    public abstract void BeginAttack();
    // 무기 활성화 종료
    public abstract void EndAttack();
    // 차지 공격
    public abstract void ChargingAttack(BaseState state);
    // 스킬
    public abstract void Skill(BaseState state);
}
