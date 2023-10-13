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
    public GameObject oneHandSwordBasic;
    public GameObject oneHandSwordRed;
    public GameObject oneHandSwordBlue;
    public GameObject oneHandSwordYellow;
    // �׽�Ʈ�� ���Ÿ� ���� ������
    public GameObject bow;
    public GameObject magicStaff;

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
        GameObject Basic = Instantiate(oneHandSwordBasic);
        GameObject Red = Instantiate(oneHandSwordRed);
        GameObject Blue = Instantiate(oneHandSwordBlue);
        GameObject Yellow = Instantiate(oneHandSwordYellow);
        PlayerStat.Instance.weaponManager.RegisterWeapon(Basic);
        PlayerStat.Instance.weaponManager.RegisterWeapon(Red);
        PlayerStat.Instance.weaponManager.RegisterWeapon(Blue);
        PlayerStat.Instance.weaponManager.RegisterWeapon(Yellow);
        PlayerStat.Instance.weaponManager.SetWeapon(Basic);
        PlayerStat.Instance.weaponManager.Weapon.EquipEffect();   // ���� ���� ����

        GameObject Bow = Instantiate(bow);
        GameObject Magic = Instantiate(magicStaff);
        PlayerStat.Instance.chargeWeaponManager.RegisterWeapon(Bow);
        PlayerStat.Instance.chargeWeaponManager.RegisterWeapon(Magic);
        PlayerStat.Instance.chargeWeaponManager.SetWeapon(Bow);
    }
}
