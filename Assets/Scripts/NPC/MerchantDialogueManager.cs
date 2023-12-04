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

        // ��Ȳ�� �°� �������� ��� ���
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
        // ��ȭ�� ������ ���� ���� (��: ��ȭâ �ݱ�)
        dialogueText.text = "";
    }
}
