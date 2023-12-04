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

    public List<Dialogue> allDialogues = new List<Dialogue>();

    private void Awake()
    {
        // ���� ��� �߰�
        allDialogues.Add(new Dialogue
        {
            situation = "����",
            dialogues = new List<string>
            {
                "�� �ʿ��� �Ŷ� �־�?",
                "���� �� �� �?",
                "��� ������ \n�׳� ��",
                "������ ��� �� \nã�ƺ�",
                "õõ�� �ѷ���",
                "���� �ʿ��Ѱž�?",
            }
        });

        allDialogues.Add(new Dialogue
        {
            situation = "�ŷ�",
            dialogues = new List<string>
            {
                "���� �ŷ�����",
                "���� �� �簥��?",
                "�������༭ ����",
                "�� �����߾�",
                "ȯ���� �����",
                "������ �� ��"
            }
        });

        // �ٸ� ��Ȳ�� ��絵 ���� ���� ������� �߰� ����
    }

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
