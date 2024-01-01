using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public List<string> dialogues = new List<string>();
    public bool usingTypingEffect;

    public void ShowDialogue()
    {
        Debug.Log("��� ���");
        if (dialogues.Count > 0)
        {
            int randomIndex = Random.Range(0, dialogues.Count); // ���� �ε��� ����
            UIManager.instance.dialogueUI.Show(dialogues[randomIndex], usingTypingEffect); // ���� ���� Ÿ���� ����Ʈ ��� ���� ����
        }
    }
}
