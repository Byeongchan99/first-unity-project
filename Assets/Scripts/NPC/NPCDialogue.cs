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
        Debug.Log("대사 출력");
        if (dialogues.Count > 0)
        {
            int randomIndex = Random.Range(0, dialogues.Count); // 랜덤 인덱스 선택
            UIManager.instance.dialogueUI.Show(dialogues[randomIndex], usingTypingEffect); // 랜덤 대사와 타이핑 이펙트 사용 여부 전달
        }
    }
}
