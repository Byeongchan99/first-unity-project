using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public static AchieveManager instance;
    public GameObject uiNotice;

    public LoadoutData[] meleeLoadouts;   // 무기 데이터
    WaitForSecondsRealtime wait;   // Time.timeScale에 영향 안 받음

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
        // 간단한 저장 기능
        PlayerPrefs.SetInt("MyData", 1);
      
        foreach (LoadoutData loadout in meleeLoadouts)
        {
            if (loadout.weaponID == 0)   // 기본 무기는 해금되어있음
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
        // 업적 잠금 해제 조건 순회하며 확인
        Debug.Log("업적 잠금 해제 조건 확인");
        foreach (LoadoutData loadout in meleeLoadouts)
        {
            UpdateAchieve(loadout);
        }
    }

    void UpdateAchieve(LoadoutData loadout)
    {
        bool isAchieve = false;
       
        // 업적 해금 확인
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
        // 업적 해금 저장
        if (isAchieve && PlayerPrefs.GetInt(loadout.weaponName) == 0)
        {
            Debug.Log(loadout.weaponName + " 잠금 해제");
            PlayerPrefs.SetInt(loadout.weaponName, 1);

            // 공지 내용 활성화
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (loadout.weaponID - 1);   // weaponID와 UI에서의 index는 1 차이
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    // 공지창 활성화
    IEnumerator NoticeRoutine()
    {
        Debug.Log("공지창 활성화");
        uiNotice.SetActive(true);
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
