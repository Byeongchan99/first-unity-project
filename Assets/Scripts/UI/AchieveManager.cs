using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public static AchieveManager instance;
    public GameObject uiNotice;

    public LoadoutData[] meleeLoadouts;   // ���� ������
    WaitForSecondsRealtime wait;   // Time.timeScale�� ���� �� ����

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        wait = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        instance = null;
    }

    void Init()
    {
        Debug.Log("AchieveManager init");
        // ������ ���� ���
        PlayerPrefs.SetInt("MyData", 1);
      
        foreach (LoadoutData loadout in meleeLoadouts)
        {
            if (loadout.weaponID == 0)   // �⺻ ����� �رݵǾ�����
            {                  
                PlayerPrefs.SetInt(loadout.weaponName, 1);
            }
            else
            {
                PlayerPrefs.SetInt(loadout.weaponName, 0);
            }
        }
    }

    void Start()
    {
        UnlockWeapon();
    }

    void UnlockWeapon()
    {
        for (int index = 0; index < meleeLoadouts.Length; index++)
        {
            string weaponName = meleeLoadouts[index].weaponName;
            bool isUnlock = PlayerPrefs.GetInt(weaponName) == 1;           
        }
    }

    public void CheckAchieve()
    {
        // ���� ��� ���� ���� ��ȸ�ϸ� Ȯ��
        Debug.Log("���� ��� ���� ���� Ȯ��");
        foreach (LoadoutData loadout in meleeLoadouts)
        {
            UpdateAchieve(loadout);
        }
    }

    void UpdateAchieve(LoadoutData loadout)
    {
        bool isAchieve = false;
       
        // ���� �ر� Ȯ��
        switch (loadout.weaponID)
        {
            case 1:
                isAchieve = PlayerStat.Instance.AttackPower >= 10;
                break;
            case 2:

                isAchieve = PlayerStat.Instance.MoveSpeed >= 300;
                break;
            case 3:

                isAchieve = PlayerStat.Instance.MaxEnergy >= 7;
                break;
        }

        // Debug.Log(isAchieve + " "  + loadout.weaponName);
        // ���� �ر� ����
        if (isAchieve && PlayerPrefs.GetInt(loadout.weaponName) == 0)
        {
            Debug.Log(loadout.weaponName + " ��� ����");
            PlayerPrefs.SetInt(loadout.weaponName, 1);

            // ���� ���� Ȱ��ȭ
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (loadout.weaponID - 1);   // weaponID�� UI������ index�� 1 ����
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    // ����â Ȱ��ȭ
    IEnumerator NoticeRoutine()
    {
        Debug.Log("����â Ȱ��ȭ");
        uiNotice.SetActive(true);
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
