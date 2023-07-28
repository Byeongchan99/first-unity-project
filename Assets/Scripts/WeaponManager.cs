using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager
{
    // 현재 무기 스크립트
    public BaseWeapon Weapon { get; private set; }
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

    // 무기 삭제
    public void UnRegisterWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon)) { 
            weapons.Remove(weapon);
        }
    }

    //// 무기 변경
    public void SetWeapon(GameObject weapon)
    {
        if (Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weapon.GetComponent<BaseWeapon>();
            weaponObject.SetActive(true);
            PlayerStat.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
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
    }
}
