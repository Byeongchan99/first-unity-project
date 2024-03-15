using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public GameObject tutorialPanel; // 튜토리얼 가이드 패널
    private bool isTriggered = false; // 이벤트가 이미 발생했는지 추적하는 플래그

    void Start()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false); // 게임 시작 시 UI 요소를 비활성화
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 이벤트 중복 방지
        if (isTriggered)
            return;

        if (other.CompareTag("Player"))
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(true); // UI 요소를 활성화
                Invoke("DeactivateTutorialPanel", 3f); // 3초 후 DeactivateTutorialPanel 메서드를 호출
                isTriggered = true;
            }
        }
    }

    // 튜토리얼 가이드 패널 비활성화
    void DeactivateTutorialPanel()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false); // UI 요소를 비활성화
        }
    }
}
