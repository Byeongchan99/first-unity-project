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
        public string situation; // ��: "����", "�ŷ�" ��
        public List<string> dialogues;
    }

    // ���� �� ��ũ��Ʈ�� ������ ������Ʈ�� �ν�����â���� ��� ���� ���� -> ���߿� ��ũ��Ʈ�� �����ϵ��� �ٲ㵵 �� ��
    public List<Dialogue> allDialogues;

    // ��Ȳ�� ���� ��� ��ȯ
    public List<string> GetDialoguesForSituation(string currentSituation)
    {
        foreach (Dialogue dialogue in allDialogues)
        {
            if (dialogue.situation == currentSituation)
            {
                return dialogue.dialogues; // ���⼭�� ��ü ��� ����� ��ȯ�մϴ�.
            }
        }
        return null;
    }
}
