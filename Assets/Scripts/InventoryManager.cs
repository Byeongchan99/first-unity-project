using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    private static InventoryManager instance;
    // �׽�Ʈ�� �Ѽհ�
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
        GameObject weapon = Instantiate(oneHandSword);
        PlayerStat.Instance.weaponManager.RegisterWeapon(weapon);
        PlayerStat.Instance.weaponManager.SetWeapon(weapon);
    }
}
