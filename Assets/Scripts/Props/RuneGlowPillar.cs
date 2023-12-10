using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.Collections.Unicode;

public class RuneGlowPillar : MonoBehaviour
{
    public GameObject[] pillars;  // 각 기둥 프리팹 배열
    public GameObject[] runes;   // 제단의 각 룬
    public static RuneGlowPillar Instance;  // 싱글톤 인스턴스

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
    }

    public void ActivateRune(int runeStageID)
    {
        // runeStageID 변수에 따라 Glow 오브젝트와 룬 활성화
        Debug.Log("Activate Pillar: " + runeStageID);
        pillars[runeStageID].transform.GetChild(0).gameObject.SetActive(true);
        runes[runeStageID].gameObject.SetActive(true);
    }
}
