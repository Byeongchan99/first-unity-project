using UnityEngine;
using System.Collections;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI txt; // UI �ؽ�Ʈ ������Ʈ
    [TextArea]
    public string fullText; // ��ü ǥ���� �ؽ�Ʈ
    public float delay = 0.1f; // �� ���� ������ ���� �ð�
    public bool complete = false; // ��ü �ؽ�Ʈ ǥ�� ����
    public bool readyForInput = false; // �Է� ���� �غ� �Ǿ����� ����

    public bool usingDialoguePanel;   // ��ȭâ ��� ���� - Ʃ�丮�󿡼� ���
    private bool justStartedDialogue = false;

    // �ʱ�ȭ
    void Init()
    {
        complete = false;
        justStartedDialogue = false;
        readyForInput = false;
    }

    // Ÿ���� ȿ�� �ڷ�ƾ
    public IEnumerator ShowText()
    {
        Init();
        justStartedDialogue = true;
        for (int i = 0; i <= fullText.Length; i++)
        {
            if (complete) // ��ü �ؽ�Ʈ�� ǥ�õǸ� �ݺ� �ߴ�
                break;
            txt.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
        txt.text = fullText; // �ݺ��� ������ ��ü �ؽ�Ʈ ǥ��

        // Ÿ���� ȿ���� �Ϸ�Ǹ� �Է� �غ� ���¸� true�� ����
        readyForInput = true;
    }

    void Update()
    {  
        if (!usingDialoguePanel)   // ��ȭâ�� ������� �ʴ� �ؽ�Ʈ�� �� ex) Ʃ�丮�� �ƾ�
        {
            if (Input.anyKeyDown)
            {
                if (!complete)
                {
                    // Ÿ������ ��ŵ�ϰ� �ٷ� ��ü �ؽ�Ʈ�� ǥ��
                    complete = true;
                    txt.text = fullText;
                }
                else
                {
                    // Ÿ������ �Ϸ�� ���¿��� Ű�� ������ readyForInput�� false�� ����
                    readyForInput = false;
                }
            }
        }
        else   // ��ȭâ�� ����ϴ� �ؽ�Ʈ�� ��
        {
            if (UIManager.instance != null)
            {
                if (UIManager.instance.dialogueUI.isOpened)   // ��ȭâ�� �������� ��
                {
                    if (Input.anyKeyDown)
                    {
                        if (!complete)
                        {
                            if (justStartedDialogue)
                            {
                                justStartedDialogue = false; // ù �Է�(NPC�� ��ȣ�ۿ�)�� ����
                            }
                            else
                            {
                                // Ÿ������ ��ŵ�ϰ� �ٷ� ��ü �ؽ�Ʈ�� ǥ��
                                complete = true;
                                txt.text = fullText;
                            }
                        }
                        else
                        {
                            // Ÿ������ �Ϸ�� ���¿��� Ű�� ������ readyForInput�� false�� ����
                            readyForInput = false;
                        }
                    }
                }
            }
        }
    }
}
