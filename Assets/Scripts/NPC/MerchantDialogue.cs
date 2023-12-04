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

    public List<Dialogue> allDialogues = new List<Dialogue>();

    private void Awake()
    {
        // 예시 대사 추가
        allDialogues.Add(new Dialogue
        {
            situation = "입장",
            dialogues = new List<string>
            {
                "뭐 필요한 거라도 있어?",
                "포션 한 잔 어때?",
                "사고 싶으면 \n그냥 사",
                "마음에 드는 걸 \n찾아봐",
                "천천히 둘러봐",
                "뭔가 필요한거야?",
            }
        });

        allDialogues.Add(new Dialogue
        {
            situation = "거래",
            dialogues = new List<string>
            {
                "좋은 거래였어",
                "뭔가 더 사갈래?",
                "구매해줘서 고마워",
                "잘 선택했어",
                "환불은 곤란해",
                "다음에 또 와"
            }
        });

        // 다른 상황의 대사도 위와 같은 방식으로 추가 가능
    }

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
