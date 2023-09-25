using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // �׽�Ʈ�� �Ѽհ� ������
    public GameObject oneHandSword;
    // �׽�Ʈ�� Ȱ ������
    public GameObject bow;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Init();   
    }

    // ���� ���� �� ������ ��� ������ ���⸦ �о���� �ʱ�ȭ �޼ҵ�
    private void Init()
    {
        /* �밭 �̷� ����
        GameObject[] weapons = Database.LoadWeapons();
        for (int i = 0; i < weapons.Length; i++)
        {
            PlayerStat.Instance.weaponManager.RegisterWeapon(weapon[i]);
        }
        */

        // �Ѽհ� �׽�Ʈ��
        // ������ ��ȯ
        GameObject weapon = Instantiate(oneHandSword);
        PlayerStat.Instance.weaponManager.RegisterWeapon(weapon);
        PlayerStat.Instance.weaponManager.SetWeapon(weapon);

        GameObject chargeWeapon = Instantiate(bow);
        PlayerStat.Instance.chargeWeaponManager.RegisterWeapon(chargeWeapon);
        PlayerStat.Instance.chargeWeaponManager.SetWeapon(chargeWeapon);
    }
}
