using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    public GameObject[] cutscenes; // �ƽ� ������Ʈ �迭
    public GameObject cutsceneBackground; // �ƽ� ��� ������Ʈ

    private int currentCutsceneIndex = 0; // ���� Ȱ��ȭ�� �ƽ� �ε���

    void Start()
    {
        // �ƾ� ������ �� ���ʿ��� UI ��Ȱ��ȭ
        UIManager.instance.healthUI.SetActive(false);
        UIManager.instance.energyUI.SetActive(false);
        UIManager.instance.goldUI.SetActive(false);

        // ��� �ƽ��� ��Ȱ��ȭ
        foreach (GameObject cutscene in cutscenes)
        {
            cutscene.SetActive(false);
        }

        // ù ��° �ƽ� Ȱ��ȭ �� �ڷ�ƾ ����
        if (cutscenes.Length > 0)
        {
            cutscenes[0].SetActive(true);
            StartCoroutine(PlayCutscene());
        }
    }

    IEnumerator PlayCutscene()
    {
        while (currentCutsceneIndex < cutscenes.Length)
        {
            GameObject currentCutscene = cutscenes[currentCutsceneIndex];
            TypingEffect typingEffect = currentCutscene.GetComponentInChildren<TypingEffect>();

            if (typingEffect != null)
            {
                // Ÿ���� ȿ���� ���� ������ ��ٸ�
                yield return new WaitUntil(() => typingEffect.complete);

                // �÷��̾� �Է��� �غ�� ������ ��ٸ�
                yield return new WaitUntil(() => typingEffect.readyForInput);
            }

            // �÷��̾��� ���� �Է��� ��ٸ�
            yield return new WaitUntil(() => Input.anyKeyDown);

            // ���� �ƽ� ��Ȱ��ȭ
            currentCutscene.SetActive(false);

            // ���� �ƽ� �ε��� ����
            currentCutsceneIndex++;

            // ���� �ƽ� Ȱ��ȭ (���� ���)
            if (currentCutsceneIndex < cutscenes.Length)
            {
                cutscenes[currentCutsceneIndex].SetActive(true);
            }
        }

        // ��� �ƽ��� ������ �ƽ� ��� ��Ȱ��ȭ
        if (cutsceneBackground != null)
        {
            cutsceneBackground.SetActive(false);
            // �ƾ� ������ �� ��Ȱ��ȭ�� UI Ȱ��ȭ
            UIManager.instance.healthUI.SetActive(true);
            UIManager.instance.energyUI.SetActive(true);
            UIManager.instance.goldUI.SetActive(true);
        }
    }
}
