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
       
        /* UI Manager에 직접 참조하면서 삭제
        if (UIManager.instance != null)   // UI 매니저를 통해 참조
        {
            UIManager.instance.SetDialogueUI(this);
        }
        else
        {
            Debug.LogError("UIManager instance not found");
        }
        */
    }

    // 자식 오브젝트인 Dialogue에 대사 업데이트
    public void Show(string dialogue, bool useTypingEffect)
    {
        rect.localScale = Vector3.one;
        isOpened = true;
        typingEffect.fullText = dialogue;

        // 타이핑 이펙트 사용 여부
        if (useTypingEffect)
        {
            typingEffect.StartCoroutine(typingEffect.ShowText());
        }
        else
        {
            typingEffect.CompleteInstantly(); // 즉시 텍스트를 전부 표시하는 함수
        }

        StartCoroutine(WaitForInput()); // 입력 대기 코루틴 시작
    }

    // 입력을 기다리는 코루틴
    private IEnumerator WaitForInput()
    {
        // 대화가 완료될 때까지 기다림
        yield return new WaitUntil(() => typingEffect.readyForInput);
        // 대화가 완료되면 사용자 입력을 기다림
        yield return new WaitUntil(() => Input.anyKeyDown);

        // 아무 키나 입력되면 대화창을 숨김
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
