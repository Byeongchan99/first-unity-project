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

    // 시작 시 타이핑 시작
    void Start()
    {
        StartCoroutine(ShowText());
    }

    // 타이핑 효과 코루틴
    IEnumerator ShowText()
    {
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
}
