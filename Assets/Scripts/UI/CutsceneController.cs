using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    public GameObject[] cutscenes; // 컷신 오브젝트 배열
    public GameObject cutsceneBackground; // 컷신 배경 오브젝트

    private int currentCutsceneIndex = 0; // 현재 활성화된 컷신 인덱스

    void Start()
    {
        // 컷씬 연출할 때 불필요한 UI 비활성화
        UIManager.instance.healthUI.SetActive(false);
        UIManager.instance.energyUI.SetActive(false);
        UIManager.instance.goldUI.SetActive(false);

        // 모든 컷신을 비활성화
        foreach (GameObject cutscene in cutscenes)
        {
            cutscene.SetActive(false);
        }

        // 첫 번째 컷신 활성화 및 코루틴 시작
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
                // 타이핑 효과가 끝날 때까지 기다림
                yield return new WaitUntil(() => typingEffect.complete);

                // 플레이어 입력이 준비될 때까지 기다림
                yield return new WaitUntil(() => typingEffect.readyForInput);
            }

            // 플레이어의 다음 입력을 기다림
            yield return new WaitUntil(() => Input.anyKeyDown);

            // 현재 컷신 비활성화
            currentCutscene.SetActive(false);

            // 다음 컷신 인덱스 증가
            currentCutsceneIndex++;

            // 다음 컷신 활성화 (있을 경우)
            if (currentCutsceneIndex < cutscenes.Length)
            {
                cutscenes[currentCutsceneIndex].SetActive(true);
            }
        }

        // 모든 컷신이 끝나면 컷신 배경 비활성화
        if (cutsceneBackground != null)
        {
            cutsceneBackground.SetActive(false);
            // 컷씬 연출할 때 비활성화한 UI 활성화
            UIManager.instance.healthUI.SetActive(true);
            UIManager.instance.energyUI.SetActive(true);
            UIManager.instance.goldUI.SetActive(true);
        }
    }
}
