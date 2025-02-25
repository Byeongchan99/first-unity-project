using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeWeaponManager
{
    // 현재 무기 스크립트
    public BaseChargeWeapon Weapon { get; private set; }
    // Action<T>를 사용하면 특정 함수를 참조하거나 "가리키는" 변수처럼 사용 가능
    public Action<GameObject> unRegisterWeapon { get; set; }
    // 무기를 쥐는 손의 트랜스폼
    public Transform chargeWeaponPosition;
    // 현재 내 무기 오브젝트
    private GameObject weaponObject;
    // 현재 WeaponManager에 등록된 무기 리스트
    private List<GameObject> weapons = new List<GameObject>();

    public ChargeWeaponManager(Transform chargeWeaponPos)
    {
        chargeWeaponPosition = chargeWeaponPos;
    }

    // 무기 등록
    public void RegisterWeapon(GameObject weapon)
    {
        if (!weapons.Contains(weapon))
        {
            BaseChargeWeapon weaponInfo = weapon.GetComponent<BaseChargeWeapon>();
            weapon.transform.SetParent(chargeWeaponPosition);
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
        // Debug.Log("Charge EquipWeapon called");
        // 현재 장착 중인 무기가 있다면 해제
        if (Weapon != null)
        {
            UnEquipWeapon(Weapon.gameObject);
        }

        // 새로운 무기를 장착
        Debug.Log("무기 장착");
        SetWeapon(newWeapon);
    }

    // 무기 장착 해제
    public void UnEquipWeapon(GameObject weapon)
    {
        // Debug.Log("Charge UnEquipWeapon called");
        if (weapon == null) return;  // weapon이 null이면 아무 작업도 수행하지 않음

        if (weapons.Contains(weapon))
        {
            Debug.Log("무기 장착 해제");
            // 등록된 무기를 리스트에서 제거
            // weapons.Remove(weapon);
            // 무기를 비활성화
            weapon.SetActive(false);
        }
    }

    /*
    // 무기 삭제
    public void UnRegisterWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon))
        {
            // 등록된 무기를 리스트에서 제거 후 해당 무기 파괴
            weapons.Remove(weapon);
            // Invoke는 unRegisterWeapon이 참조하는 메서드(현재 Destroy(weapon))를 호출하라는 의미
            unRegisterWeapon.Invoke(weapon);
        }
    }
    */

    // 무기 변경
    // 현재 내가 사용하는 무기만 활성화, 나머지 무기들은 비활성화된 채로 쥐고 있음
    public void SetWeapon(GameObject weapon)
    {
        BaseChargeWeapon weaponComponent = weapon.GetComponent<BaseChargeWeapon>();

        if (Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weapon.GetComponent<BaseChargeWeapon>();
            weaponObject.SetActive(true);
            weaponObject.GetComponentInChildren<SpriteRenderer>().enabled = false; // 자식 오브젝트의 스프라이트 렌더러 비활성화
            // PlayerStat.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
            return;
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            BaseChargeWeapon currentWeaponComponent = weapons[i].GetComponent<BaseChargeWeapon>();
            if (currentWeaponComponent.WeaponID == weaponComponent.WeaponID)
            {
                weaponObject = weapons[i];
                weaponObject.SetActive(true);
                weaponObject.GetComponentInChildren<SpriteRenderer>().enabled = false; // 자식 오브젝트의 스프라이트 렌더러 비활성화
                Weapon = weaponObject.GetComponent<BaseChargeWeapon>();
                // PlayerStat.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
                continue;
            }
            weapons[i].SetActive(false);
        }
    }
}
