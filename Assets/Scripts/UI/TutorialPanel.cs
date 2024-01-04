using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public GameObject tutorialPanel; // Ʃ�丮�� ���̵� �г�
    private bool isTriggered = false; // �̺�Ʈ�� �̹� �߻��ߴ��� �����ϴ� �÷���.

    void Start()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false); // ���� ���� �� UI ��Ҹ� ��Ȱ��ȭ�մϴ�.
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �̺�Ʈ�� �̹� �߻������� �ƹ� �͵� ���� �ʽ��ϴ�.
        if (isTriggered)
            return;

        if (other.CompareTag("Player"))
        {
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(true); // UI ��Ҹ� Ȱ��ȭ�մϴ�.
                Invoke("DeactivateTutorialPanel", 3f); // 3�� �� DeactivateTutorialPanel �޼��带 ȣ��
                isTriggered = true;
            }
        }
    }

    // Ʃ�丮�� ���̵� �г� ��Ȱ��ȭ
    void DeactivateTutorialPanel()
    {
        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(false); // UI ��Ҹ� ��Ȱ��ȭ�մϴ�.
        }
    }
}
