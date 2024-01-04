using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.Collections.Unicode;

public class RuneGlowPillar : MonoBehaviour
{
    public static RuneGlowPillar Instance;  // �̱��� �ν��Ͻ�

    public GameObject[] portals;   // �� �������� ��Ż
    public GameObject[] pillars;  // �� ��� ������ �迭
    public GameObject[] runes;   // ������ �� ��
    public GameObject altarPortal;   // ���� ��Ż

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
        // ��� ��Ż ������Ʈ Ȱ��ȭ
        foreach (GameObject portal in portals)
        {
            portal.gameObject.SetActive(true);
        }

        // ��� ����� Glow ������Ʈ�� ��Ȱ��ȭ
        foreach (GameObject pillar in pillars)
        {
            pillar.transform.GetChild(0).gameObject.SetActive(false);
        }

        // ��� �� ������Ʈ�� ��Ȱ��ȭ
        foreach (GameObject rune in runes)
        {
            rune.gameObject.SetActive(false);
        }

        // altar�� Portal �ڽ� ������Ʈ ��Ȱ��ȭ
        altarPortal.SetActive(false);
    }

    public void ActivateRune(int runeStageID)
    {
        // runeStageID ������ ���� Glow ������Ʈ�� �� Ȱ��ȭ
        Debug.Log("Activate Pillar: " + runeStageID);
        portals[runeStageID].gameObject.SetActive(false);
        pillars[runeStageID].transform.GetChild(0).gameObject.SetActive(true);
        runes[runeStageID].gameObject.SetActive(true);

        // ��� �� ���������� Ŭ���� �ߴ��� Ȯ�� �� altar�� Portal ��ũ��Ʈ ������Ʈ Ȱ��ȭ
        StageManager.Instance.updateAllStageCleared();

        if (StageManager.Instance.allStageCleared)
        {
            // altar�� Portal �ڽ� ������Ʈ ��Ȱ��ȭ
            altarPortal.SetActive(true);
        }
    }
}
