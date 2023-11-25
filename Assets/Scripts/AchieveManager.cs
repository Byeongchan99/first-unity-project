using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public static AchieveManager instance;

    public GameObject[] lockWeapon;
    public GameObject[] unlockWeapon;
    public GameObject uiNotice;

    enum Achieve { UnlockRedSword, UnlockYellowSword, UnlockBlueSword }
    Achieve[] achieves;
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

        achieves = (Achieve[])System.Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        // ������ ���� ���
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achieve Achieve in achieves)
        {
            PlayerPrefs.SetInt(Achieve.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockWeapon();
    }

    void UnlockWeapon()
    {
        for (int index = 0; index < lockWeapon.Length; index++)
        {
            string AchieveName = achieves[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(AchieveName) == 1;
            lockWeapon[index].SetActive(!isUnlock);
            unlockWeapon[index].SetActive(isUnlock);
        }
    }

    public void CheckAchieve()
    {
        // ���� ��� ���� ���� ��ȸ�ϸ� Ȯ��
        foreach (Achieve achieve in achieves)
        {
            UpdateAchieve(achieve);
        }
    }

    void UpdateAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        // ���� �ر� Ȯ��
        switch (achieve)
        {
            case Achieve.UnlockRedSword:
                if (GameManager.instance.isLive)
                {
                    isAchieve = PlayerStat.Instance.AttackPower >= 10;
                }
                break;
            case Achieve.UnlockYellowSword:
                if (GameManager.instance.isLive)
                {
                    isAchieve = PlayerStat.Instance.MoveSpeed >= 300;
                }
                break;
            case Achieve.UnlockBlueSword:
                if (GameManager.instance.isLive)
                {
                    isAchieve = PlayerStat.Instance.MaxEnergy >= 7;
                }
                break;
        }

        // ���� �ر� ����
        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            // ���� ���� Ȱ��ȭ
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achieve;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    // ����â Ȱ��ȭ
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}
