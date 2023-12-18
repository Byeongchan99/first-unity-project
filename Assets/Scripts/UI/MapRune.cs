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

    // �ʿ� �� Ȱ��ȭ
    public void ActivateRune(int runeStageID)
    {
        if (runeStageID >= 0 && runeStageID < runes.Length)
        {
            runes[runeStageID].gameObject.SetActive(true);
            StartCoroutine(FadeIn(runes[runeStageID], 3.0f)); // ���̵��� �ڷ�ƾ ȣ��
        }
        else
        {
            Debug.LogError("Invalid runeStageID: " + runeStageID);
        }
    }

    // ���̵��� ȿ�� �ڷ�ƾ
    IEnumerator FadeIn(Image image, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float alpha = elapsedTime / duration;
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            elapsedTime += Time.unscaledDeltaTime; // Time.timeScale�� ������ ���� �ʴ� deltaTime ���
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1); // ���� ���İ� 1�� ����
        yield return new WaitForSecondsRealtime(1.0f);
        UIManager.instance.mapBackgroundUI.Hide();   // �� �ݱ�
    }
}
