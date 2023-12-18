using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Slider loadingBar; // 로딩바로 사용할 Slider
    public float duration = 3.0f; // 로딩바가 채워지는 데 걸리는 시간 (3초)

    private float startTime;

    void OnEnable()
    {
        startTime = Time.time; // 로딩 시작 시간
        loadingBar.value = 0; // 로딩바 초기화
    }

    void Update()
    {
        if (loadingBar.value < 1)
        {
            float timeElapsed = Time.time - startTime;
            loadingBar.value = Mathf.Clamp(timeElapsed / duration, 0, 1); // 3초 동안 로딩바 채우기
        }
    }
}
