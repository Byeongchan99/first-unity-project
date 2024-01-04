using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.Collections.Unicode;

public class RuneGlowPillar : MonoBehaviour
{
    public static RuneGlowPillar Instance;  // 싱글톤 인스턴스

    public GameObject[] portals;   // 룬 스테이지 포탈
    public GameObject[] pillars;  // 각 기둥 프리팹 배열
    public GameObject[] runes;   // 제단의 각 룬
    public GameObject altarPortal;   // 제단 포탈

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 모든 포탈 오브젝트 활성화
        foreach (GameObject portal in portals)
        {
            portal.gameObject.SetActive(true);
        }

        // 모든 기둥의 Glow 오브젝트를 비활성화
        foreach (GameObject pillar in pillars)
        {
            pillar.transform.GetChild(0).gameObject.SetActive(false);
        }

        // 모든 룬 오브젝트를 비활성화
        foreach (GameObject rune in runes)
        {
            rune.gameObject.SetActive(false);
        }

        // altar의 Portal 자식 오브젝트 비활성화
        altarPortal.SetActive(false);
    }

    public void ActivateRune(int runeStageID)
    {
        // runeStageID 변수에 따라 Glow 오브젝트와 룬 활성화
        Debug.Log("Activate Pillar: " + runeStageID);
        portals[runeStageID].gameObject.SetActive(false);
        pillars[runeStageID].transform.GetChild(0).gameObject.SetActive(true);
        runes[runeStageID].gameObject.SetActive(true);

        // 모든 룬 스테이지를 클리어 했는지 확인 후 altar의 Portal 스크립트 오브젝트 활성화
        StageManager.Instance.updateAllStageCleared();

        if (StageManager.Instance.allStageCleared)
        {
            // altar의 Portal 자식 오브젝트 비활성화
            altarPortal.SetActive(true);
        }
    }
}
