using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public static BackgroundMusicController Instance { get; private set; }
    public AudioSource musicSource;       // 배경음악을 재생할 AudioSource
    public AudioClip[] musicClips;        // 인스펙터에서 설정할 배경음악 클립 배열
    public float fadeTime = 1.5f;         // 페이드인/페이드아웃에 걸리는 시간

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (musicSource == null) { musicSource = gameObject.AddComponent<AudioSource>(); }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        Instance = null;
    }

    // 배경음악을 바꾸는 메서드
    public void ChangeMusic(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < musicClips.Length)
        {
            StartCoroutine(FadeChangeMusic(musicClips[clipIndex]));
        }
    }

    // 페이드 효과와 함께 음악을 바꾸는 코루틴
    IEnumerator FadeChangeMusic(AudioClip newClip)
    {
        if (musicSource.isPlaying)
        {
            // 페이드아웃
            for (float t = 0; t < fadeTime; t += Time.deltaTime)
            {
                musicSource.volume = (1 - (t / fadeTime));
                yield return null;
            }
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // 페이드인
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = (t / fadeTime);
            yield return null;
        }

        musicSource.volume = 1f; // 페이드인이 끝난 후 볼륨을 다시 1로 설정
    }

    // 배경음악을 멈추는 메서드
    public void StopMusic()
    {
        StartCoroutine(FadeOutMusic());
    }

    // 페이드아웃 효과와 함께 음악을 멈추는 코루틴
    IEnumerator FadeOutMusic()
    {
        // 페이드아웃
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = 1 - (t / fadeTime);
            yield return null;
        }

        musicSource.Stop(); // 음악 정지
        musicSource.volume = 1f; // 볼륨을 원래대로 복구 (다음 재생을 위해)
    }
}
