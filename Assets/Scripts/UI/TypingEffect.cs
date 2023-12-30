using UnityEngine;
using System.Collections;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI txt; // UI 텍스트 컴포넌트
    [TextArea]
    public string fullText; // 전체 표시할 텍스트
    public float delay = 0.1f; // 각 문자 사이의 지연 시간
    public bool complete = false; // 전체 텍스트 표시 여부
    public bool readyForInput = false; // 입력 받을 준비가 되었는지 여부

    public bool usingDialoguePanel;   // 대화창 사용 여부 - 튜토리얼에서 사용
    private bool justStartedDialogue = false;

    // 초기화
    void Init()
    {
        complete = false;
        justStartedDialogue = false;
        readyForInput = false;
    }

    // 타이핑 효과 코루틴
    public IEnumerator ShowText()
    {
        Init();
        justStartedDialogue = true;
        for (int i = 0; i <= fullText.Length; i++)
        {
            if (complete) // 전체 텍스트가 표시되면 반복 중단
                break;
            txt.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(delay);
        }
        txt.text = fullText; // 반복이 끝나면 전체 텍스트 표시

        // 타이핑 효과가 완료되면 입력 준비 상태를 true로 설정
        readyForInput = true;
    }

    void Update()
    {  
        if (!usingDialoguePanel)   // 대화창을 사용하지 않는 텍스트일 때 ex) 튜토리얼 컷씬
        {
            if (Input.anyKeyDown)
            {
                if (!complete)
                {
                    // 타이핑을 스킵하고 바로 전체 텍스트를 표시
                    complete = true;
                    txt.text = fullText;
                }
                else
                {
                    // 타이핑이 완료된 상태에서 키를 누르면 readyForInput을 false로 설정
                    readyForInput = false;
                }
            }
        }
        else   // 대화창을 사용하는 텍스트일 때
        {
            if (UIManager.instance != null)
            {
                if (UIManager.instance.dialogueUI.isOpened)   // 대화창이 열려있을 때
                {
                    if (Input.anyKeyDown)
                    {
                        if (!complete)
                        {
                            if (justStartedDialogue)
                            {
                                justStartedDialogue = false; // 첫 입력(NPC와 상호작용)을 무시
                            }
                            else
                            {
                                // 타이핑을 스킵하고 바로 전체 텍스트를 표시
                                complete = true;
                                txt.text = fullText;
                            }
                        }
                        else
                        {
                            // 타이핑이 완료된 상태에서 키를 누르면 readyForInput을 false로 설정
                            readyForInput = false;
                        }
                    }
                }
            }
        }
    }
}
