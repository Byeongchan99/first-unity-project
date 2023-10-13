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
        GameObject Basic = Instantiate(oneHandSwordBasic);
        GameObject Red = Instantiate(oneHandSwordRed);
        GameObject Blue = Instantiate(oneHandSwordBlue);
        GameObject Yellow = Instantiate(oneHandSwordYellow);
        PlayerStat.Instance.weaponManager.RegisterWeapon(Basic);
        PlayerStat.Instance.weaponManager.RegisterWeapon(Red);
        PlayerStat.Instance.weaponManager.RegisterWeapon(Blue);
        PlayerStat.Instance.weaponManager.RegisterWeapon(Yellow);
        PlayerStat.Instance.weaponManager.SetWeapon(Basic);
        PlayerStat.Instance.weaponManager.Weapon.EquipEffect();   // 무기 스텟 적용

        GameObject Bow = Instantiate(bow);
        GameObject Magic = Instantiate(magicStaff);
        PlayerStat.Instance.chargeWeaponManager.RegisterWeapon(Bow);
        PlayerStat.Instance.chargeWeaponManager.RegisterWeapon(Magic);
        PlayerStat.Instance.chargeWeaponManager.SetWeapon(Bow);
    }
}
