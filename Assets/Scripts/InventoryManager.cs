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

    // 테스트용 한손검 프리팹
    public GameObject oneHandSwordBasic;
    public GameObject oneHandSwordRed;
    public GameObject oneHandSwordBlue;
    public GameObject oneHandSwordYellow;
    // 테스트용 원거리 무기 프리팹
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

    // 게임 시작 후 유저의 사용 가능한 무기를 읽어오는 초기화 메소드
    private void Init()
    {
        /* 대강 이런 로직
        GameObject[] weapons = Database.LoadWeapons();
        for (int i = 0; i < weapons.Length; i++)
        {
            PlayerStat.Instance.weaponManager.RegisterWeapon(weapon[i]);
        }
        */

        // 한손검 테스트용
        // 프리팹 소환
        GameObject weapon = Instantiate(oneHandSwordYellow);
        PlayerStat.Instance.weaponManager.RegisterWeapon(weapon);
        PlayerStat.Instance.weaponManager.SetWeapon(weapon);
        PlayerStat.Instance.weaponManager.Weapon.EquipEffect();

        GameObject chargeWeapon = Instantiate(bow);
        PlayerStat.Instance.chargeWeaponManager.RegisterWeapon(chargeWeapon);
        PlayerStat.Instance.chargeWeaponManager.SetWeapon(chargeWeapon);
    }
}
