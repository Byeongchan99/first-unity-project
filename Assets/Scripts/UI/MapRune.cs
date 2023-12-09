using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRune : MonoBehaviour
{
    public Image[] runes;  // 자식 오브젝트로 있는 UI 이미지 배열

    void Start()
    {
        // 모든 룬 오브젝트를 비활성화
        foreach (var rune in runes)
        {
            rune.gameObject.SetActive(false);
        }
    }

    public void ActivateRune(int runeStageID)
    {
        // runeStageID 변수에 따라 룬 오브젝트 활성화
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
