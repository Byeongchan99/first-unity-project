using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MerchantDialogue : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogue
    {
        public string situation; // 예: "입장", "거래" 등
        public List<string> dialogues;
    }

    // 현재 이 스크립트가 장착된 컴포넌트의 인스펙터창에서 대사 편집 가능 -> 나중에 스크립트로 관리하도록 바꿔도 될 듯
    public List<Dialogue> allDialogues;

    // 상황에 따라 대사 반환
    public List<string> GetDialoguesForSituation(string currentSituation)
    {
        foreach (Dialogue dialogue in allDialogues)
        {
            if (dialogue.situation == currentSituation)
            {
                return dialogue.dialogues; // 여기서는 전체 대사 목록을 반환합니다.
            }
        }
        return null;
    }
}
