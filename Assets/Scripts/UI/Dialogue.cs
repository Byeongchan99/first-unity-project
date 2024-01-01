using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static MerchantDialogue;

public class Dialogue : MonoBehaviour
{
    RectTransform rect;
    public TypingEffect typingEffect;
    public bool isOpened = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
       
        /* UI Manager�� ���� �����ϸ鼭 ����
        if (UIManager.instance != null)   // UI �Ŵ����� ���� ����
        {
            UIManager.instance.SetDialogueUI(this);
        }
        else
        {
            Debug.LogError("UIManager instance not found");
        }
        */
    }

    // �ڽ� ������Ʈ�� Dialogue�� ��� ������Ʈ
    public void Show(string dialogue, bool useTypingEffect)
    {
        rect.localScale = Vector3.one;
        isOpened = true;
        typingEffect.fullText = dialogue;

        // Ÿ���� ����Ʈ ��� ����
        if (useTypingEffect)
        {
            typingEffect.StartCoroutine(typingEffect.ShowText());
        }
        else
        {
            typingEffect.CompleteInstantly(); // ��� �ؽ�Ʈ�� ���� ǥ���ϴ� �Լ�
        }

        StartCoroutine(WaitForInput()); // �Է� ��� �ڷ�ƾ ����
    }

    // �Է��� ��ٸ��� �ڷ�ƾ
    private IEnumerator WaitForInput()
    {
        // ��ȭ�� �Ϸ�� ������ ��ٸ�
        yield return new WaitUntil(() => typingEffect.readyForInput);
        // ��ȭ�� �Ϸ�Ǹ� ����� �Է��� ��ٸ�
        yield return new WaitUntil(() => Input.anyKeyDown);

        // �ƹ� Ű�� �ԷµǸ� ��ȭâ�� ����
        Hide();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        isOpened = false;
        //GameManager.instance.Resume();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }
}
