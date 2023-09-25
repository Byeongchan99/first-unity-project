using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager
{
    // ���� ���� ��ũ��Ʈ
    public BaseWeapon Weapon { get; private set; }
    // Action<T>�� ����ϸ� Ư�� �Լ��� �����ϰų� "����Ű��" ����ó�� ��� ����
    public Action<GameObject> unRegisterWeapon { get; set; }
    // ���⸦ ��� ���� Ʈ������
    private Transform handPosition;
    // ���� �� ���� ������Ʈ
    private GameObject weaponObject;
    // ���� WeaponManager�� ��ϵ� ���� ����Ʈ
    private List<GameObject> weapons = new List<GameObject>();

    public WeaponManager(Transform hand)
    {
        handPosition = hand;
    }

    // ���� ���
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

    // ���� ��ü - UI ��ư���� ȣ���� �޼���
    public void EquipWeapon(GameObject newWeapon)
    {
        // ���� ���� ���� ���Ⱑ �ִٸ� ����
        if (Weapon != null)
        {
            UnEquipWeapon(Weapon.gameObject);
        }

        // ���ο� ���⸦ ����
        SetWeapon(newWeapon);
    }

    // ���� ���� ����
    public void UnEquipWeapon(GameObject weapon)
    {
        if (weapon == null) return;  // weapon�� null�̸� �ƹ� �۾��� �������� ����

        if (weapons.Contains(weapon))
        {
            // ��ϵ� ���⸦ ����Ʈ���� ����
            // weapons.Remove(weapon);
            // ���⸦ ��Ȱ��ȭ
            weapon.SetActive(false);
        }
    }

    // ���� ����
    // ���� ���� ����ϴ� ���⸸ Ȱ��ȭ, ������ ������� ��Ȱ��ȭ�� ä�� ��� ����
    public void SetWeapon(GameObject weapon)
    {
        if (Weapon == null)
        {
            weaponObject = weapon;
            Weapon = weapon.GetComponent<BaseWeapon>();
            weaponObject.SetActive(true);
            PlayerStat.Instance.animator.runtimeAnimatorController = Weapon.WeaponAnimator;

            // Weapon�� ������ �� PlayerAttackArea �ʱ�ȭ
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

        // Weapon ���� �� PlayerAttackArea �ʱ�ȭ
        InitializeAttackArea();
    }

    // ���� ���� �ʱ�ȭ
    private void InitializeAttackArea()
    {
        PlayerAttackArea attackArea = PlayerStat.Instance.gameObject.transform.Find("AttackArea").GetComponent<PlayerAttackArea>();
        if (attackArea != null)
        {
            attackArea.Initialize();
        }
    }
}
