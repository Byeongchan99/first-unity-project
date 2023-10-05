using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeWeaponManager
{
    // ���� ���� ��ũ��Ʈ
    public BaseChargeWeapon Weapon { get; private set; }
    // Action<T>�� ����ϸ� Ư�� �Լ��� �����ϰų� "����Ű��" ����ó�� ��� ����
    public Action<GameObject> unRegisterWeapon { get; set; }
    // ���⸦ ��� ���� Ʈ������
    private Transform chargeWeaponPosition;
    // ���� �� ���� ������Ʈ
    private GameObject weaponObject;
    // ���� WeaponManager�� ��ϵ� ���� ����Ʈ
    private List<GameObject> weapons = new List<GameObject>();

    public ChargeWeaponManager(Transform chargeWeaponPos)
    {
        chargeWeaponPosition = chargeWeaponPos;
    }

    // ���� ���
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

    // ���� ����
    public void UnRegisterWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon))
        {
            // ��ϵ� ���⸦ ����Ʈ���� ���� �� �ش� ���� �ı�
            weapons.Remove(weapon);
            // Invoke�� unRegisterWeapon�� �����ϴ� �޼���(���� Destroy(weapon))�� ȣ���϶�� �ǹ�
            unRegisterWeapon.Invoke(weapon);
        }
    }

    // ���� ����
    // ���� ���� ����ϴ� ���⸸ Ȱ��ȭ, ������ ������� ��Ȱ��ȭ�� ä�� ��� ����
    public void SetWeapon(GameObject weapon)
    {
        if (Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weapon.GetComponent<BaseChargeWeapon>();
            weaponObject.SetActive(true);
            weaponObject.GetComponentInChildren<SpriteRenderer>().enabled = false; // �ڽ� ������Ʈ�� ��������Ʈ ������ ��Ȱ��ȭ
            PlayerStat.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
            return;
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i].Equals(weapon))
            {
                weaponObject = weapon;
                weaponObject.SetActive(true);
                weaponObject.GetComponentInChildren<SpriteRenderer>().enabled = false; // �ڽ� ������Ʈ�� ��������Ʈ ������ ��Ȱ��ȭ
                Weapon = weapon.GetComponent<BaseChargeWeapon>();
                PlayerStat.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;
                continue;
            }
            weapons[i].SetActive(false);
        }
    }
}
