using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager
{
    // 현재 무기 스크립트
    public BaseWeapon Weapon { get; private set; }
    // Action<T>를 사용하면 특정 함수를 참조하거나 "가리키는" 변수처럼 사용 가능
    public Action<GameObject> unRegisterWeapon { get; set; }
    // 무기를 쥐는 손의 트랜스폼
    private Transform handPosition;
    // 현재 내 무기 오브젝트
    private GameObject weaponObject;
    // 현재 WeaponManager에 등록된 무기 리스트
    private List<GameObject> weapons = new List<GameObject>();

    public WeaponManager(Transform hand)
    {
        handPosition = hand;
    }

    // 무기 등록
    public void RegisterWeapon(GameObject weapon)
    {
        if (!weapons.Contains(weapon))
        {
            BaseWeapon weaponInfo = weapon.GetComponent<BaseWeapon>();
            weapon.transform.SetParent(handPosition);
            weapon.transform.localPosition = weaponInfo.HandleData.localPosition;
            weapon.transform.localEulerAngles = weaponInfo.HandleData.localRotation;
            weapon.transform.localScale = weaponInfo.HandleData.localScale;
            weapons.Add(weapon);
            weapon.SetActive(false);
        }
    }

    // 무기 교체 - UI 버튼에서 호출한 메서드
    public void EquipWeapon(GameObject newWeapon)
    {
        // 현재 장착 중인 무기가 있다면 해제
        if (Weapon != null)
        {
            UnEquipWeapon(Weapon.gameObject);
        }

        // 새로운 무기를 장착
        SetWeapon(newWeapon);
    }

    // 무기 장착 해제
    public void UnEquipWeapon(GameObject weapon)
    {
        if (weapon == null) return;  // weapon이 null이면 아무 작업도 수행하지 않음

        if (weapons.Contains(weapon))
        {
            // 등록된 무기를 리스트에서 제거
            // weapons.Remove(weapon);
            // 무기를 비활성화
            weapon.SetActive(false);
        }
    }

    // 무기 장착
    // 현재 내가 사용하는 무기만 활성화, 나머지 무기들은 비활성화된 채로 쥐고 있음
    public void SetWeapon(GameObject weapon)
    {
        if (Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weapon.GetComponent<BaseWeapon>();
            weaponObject.SetActive(true);
            PlayerStat.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;

            // Weapon이 설정된 후 PlayerAttackArea 초기화
            InitializeAttackArea();

            return;
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].Equals(weapon))
            {
                weaponObject = weapon;
                weaponObject.SetActive(true);
                Weapon = weapon.GetComponent<BaseWeapon>();
                PlayerStat.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
                continue;
            }
            weapons[i].SetActive(false);
        }

        // Weapon 변경 후 PlayerAttackArea 초기화
        InitializeAttackArea();
    }

    // 공격 범위 초기화
    private void InitializeAttackArea()
    {
        PlayerAttackArea attackArea = PlayerStat.Instance.gameObject.transform.Find("AttackArea").GetComponent<PlayerAttackArea>();
        if (attackArea != null)
        {
            attackArea.Initialize();
        }
    }
}
