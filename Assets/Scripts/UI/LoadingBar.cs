using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Slider loadingBar; // �ε��ٷ� ����� Slider
    public float duration = 3.0f; // �ε��ٰ� ä������ �� �ɸ��� �ð� (3��)

    private float startTime;

    void OnEnable()
    {
        startTime = Time.time; // �ε� ���� �ð�
        loadingBar.value = 0; // �ε��� �ʱ�ȭ
    }

    void Update()
    {
        if (loadingBar.value < 1)
        {
            float timeElapsed = Time.time - startTime;
            loadingBar.value = Mathf.Clamp(timeElapsed / duration, 0, 1); // 3�� ���� �ε��� ä���
        }
    }
}
