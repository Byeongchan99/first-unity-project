using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private static InventoryManager instance;
    // 테스트용 한손검
    public GameObject oneHandSword;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
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
        GameObject weapon = Instantiate(oneHandSword);
        PlayerStat.Instance.weaponManager.RegisterWeapon(weapon);
        PlayerStat.Instance.weaponManager.SetWeapon(weapon);
    }
}
