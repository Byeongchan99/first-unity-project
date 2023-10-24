using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MerchantDialogueManager : MonoBehaviour
{
    public MerchantDialogue npcDialogue;
    public TextMeshProUGUI dialogueText;
    private List<string> currentDialogues;

    public void StartDialogue(string situation)
    {
        currentDialogues = npcDialogue.GetDialoguesForSituation(situation);

        // 상황에 맞게 랜덤으로 대사 출력
        if (currentDialogues != null && currentDialogues.Count > 0)
        {
            int randomIndex = Random.Range(0, currentDialogues.Count);
            dialogueText.text = currentDialogues[randomIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        // 대화가 끝났을 때의 동작 (예: 대화창 닫기)
        dialogueText.text = "";
    }
}
