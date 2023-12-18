using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapRune : MonoBehaviour
{
    public Image[] runes;

    void Start()
    {
        foreach (var rune in runes)
        {
            rune.gameObject.SetActive(false);
        }
    }

    // 맵에 룬 활성화
    public void ActivateRune(int runeStageID)
    {
        if (runeStageID >= 0 && runeStageID < runes.Length)
        {
            runes[runeStageID].gameObject.SetActive(true);
            StartCoroutine(FadeIn(runes[runeStageID], 3.0f)); // 페이드인 코루틴 호출
        }
        else
        {
            Debug.LogError("Invalid runeStageID: " + runeStageID);
        }
    }

    // 페이드인 효과 코루틴
    IEnumerator FadeIn(Image image, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float alpha = elapsedTime / duration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            elapsedTime += Time.unscaledDeltaTime; // Time.timeScale의 영향을 받지 않는 deltaTime 사용
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1); // 최종 알파값 1로 설정
        yield return new WaitForSecondsRealtime(1.0f);
        UIManager.instance.mapBackgroundUI.Hide();   // 맵 닫기
    }
}
