using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public abstract class BaseChargeWeapon : MonoBehaviour
{
    // 이 무기를 쥘 때 손의 로컬 좌표 정보
    public WeaponHandleData HandleData { get { return weaponhandleData; } }
    // 이 무기를 사용할 때의 애니메이터
    public RuntimeAnimatorController WeaponAnimator { get { return weaponAnimator; } }

    public string WeaponName { get { return weaponName; } }
    public float AttackDamage { get { return attackDamage; } }
    public float AttackSpeed { get { return attackSpeed; } }
    public int BulletID { get { return bulletID; } }
    public float DamageCoefficient { get { return damageCoefficient; } }

    [Header("생성 정보"), Tooltip("무기를 쥐었을 때의 Local Transform 정보")]
    [SerializeField] protected WeaponHandleData weaponhandleData;

    [Header("무기 정보")]
    [SerializeField] protected RuntimeAnimatorController weaponAnimator;
    [SerializeField] protected string weaponName;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int bulletID;   // 사용하는 총알의 프리팹 ID
    [SerializeField] protected float damageCoefficient;
   
    public void SetWeaponData(string weaponName, float attackDamage, float attackSpeed, int bulletID, float damageCoefficient)
    {
        this.weaponName = weaponName;
        this.attackDamage = attackDamage;
        this.attackSpeed = attackSpeed;
        this.bulletID = bulletID;
        this.damageCoefficient = damageCoefficient;
    }

    // 기본 공격
    public abstract void Attack(BaseState state);
    // 무기 활성화 시작
    public abstract void BeginAttack();
    // 무기 활성화 종료
    public abstract void EndAttack();
    // 무기 위치 마우스 따라 회전
    public abstract void RotateWeaponTowardsMouse();
    // 손 위치 변경
    public abstract void UpdateWeaponAndHandPosition(float chargeTime);
    // 차지 공격
    public abstract void ChargingAttack(BaseState state, Vector2 dir, int chargeLevel);
    // 스킬
    public abstract void Skill(BaseState state);
}
