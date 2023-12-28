using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    public static BackgroundMusicController Instance { get; private set; }
    public AudioSource musicSource;       // ��������� ����� AudioSource
    public AudioClip[] musicClips;        // �ν����Ϳ��� ������ ������� Ŭ�� �迭
    public float fadeTime = 1.5f;         // ���̵���/���̵�ƿ��� �ɸ��� �ð�

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

    // ��������� �ٲٴ� �޼���
    public void ChangeMusic(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < musicClips.Length)
        {
            StartCoroutine(FadeChangeMusic(musicClips[clipIndex]));
        }
    }

    // ���̵� ȿ���� �Բ� ������ �ٲٴ� �ڷ�ƾ
    IEnumerator FadeChangeMusic(AudioClip newClip)
    {
        if (musicSource.isPlaying)
        {
            // ���̵�ƿ�
            for (float t = 0; t < fadeTime; t += Time.deltaTime)
            {
                musicSource.volume = (1 - (t / fadeTime));
                yield return null;
            }
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // ���̵���
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = (t / fadeTime);
            yield return null;
        }

        musicSource.volume = 1f; // ���̵����� ���� �� ������ �ٽ� 1�� ����
    }

    // ��������� ���ߴ� �޼���
    public void StopMusic()
    {
        StartCoroutine(FadeOutMusic());
    }

    // ���̵�ƿ� ȿ���� �Բ� ������ ���ߴ� �ڷ�ƾ
    IEnumerator FadeOutMusic()
    {
        // ���̵�ƿ�
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            musicSource.volume = 1 - (t / fadeTime);
            yield return null;
        }

        musicSource.Stop(); // ���� ����
        musicSource.volume = 1f; // ������ ������� ���� (���� ����� ����)
    }
}
