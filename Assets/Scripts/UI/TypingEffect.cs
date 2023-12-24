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

    // ���� �� Ÿ���� ����
    void Start()
    {
        StartCoroutine(ShowText());
    }

    // Ÿ���� ȿ�� �ڷ�ƾ
    IEnumerator ShowText()
    {
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
}
