using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRune : MonoBehaviour
{
    public Image[] runes;  // �ڽ� ������Ʈ�� �ִ� UI �̹��� �迭

    void Start()
    {
        // ��� �� ������Ʈ�� ��Ȱ��ȭ
        foreach (var rune in runes)
        {
            rune.gameObject.SetActive(false);
        }
    }

    public void ActivateRune(int runeStageID)
    {
        // runeStageID ������ ���� �� ������Ʈ Ȱ��ȭ
        if (runeStageID >= 0 && runeStageID < runes.Length)
        {
            runes[runeStageID].gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Invalid runeStageID: " + runeStageID);
        }
    }
}
