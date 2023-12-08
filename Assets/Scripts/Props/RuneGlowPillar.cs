using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneGlowPillar : MonoBehaviour
{
    public GameObject[] pillars;  // 각 기둥 프리팹 배열
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
    }

    public void ActivatePillar(int runeStageID)
    {
        // runeStageID 변수에 따라 Glow 오브젝트 활성화
        Debug.Log("Activate Pillar: " + runeStageID);
        pillars[runeStageID].transform.GetChild(0).gameObject.SetActive(true);
    }
}
