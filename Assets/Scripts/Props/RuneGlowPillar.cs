using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.Collections.Unicode;

public class RuneGlowPillar : MonoBehaviour
{
    public GameObject[] pillars;  // �� ��� ������ �迭
    public GameObject[] runes;   // ������ �� ��
    public static RuneGlowPillar Instance;  // �̱��� �ν��Ͻ�

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
    }

    public void ActivateRune(int runeStageID)
    {
        // runeStageID ������ ���� Glow ������Ʈ�� �� Ȱ��ȭ
        Debug.Log("Activate Pillar: " + runeStageID);
        pillars[runeStageID].transform.GetChild(0).gameObject.SetActive(true);
        runes[runeStageID].gameObject.SetActive(true);
    }
}
