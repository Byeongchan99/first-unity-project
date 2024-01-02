using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource source;
    public AudioClip[] audioClips;   // ��Ÿ ȿ����
    public AudioClip[] audioClipsUI;   // UI ȿ���� - UI ��ư Ŭ��, ���� ����, �� ����, �� �ݱ�, �Ͻ����� �޴� Ȱ��ȭ, �Ͻ����� �޴� ��Ȱ��ȭ, ���� �ر�, �ź�, �����Ƽ ����

    void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� �ı����� �ʵ��� ����
            source = GetComponent<AudioSource>();
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

    // ��Ÿ ȿ���� ����
    public void PlaySound(int audioIndex)
    {
        Debug.Log("ȿ���� ����" + audioIndex);
        if (audioIndex >= 0 && audioIndex < audioClips.Length)
        {
            source.PlayOneShot(audioClips[audioIndex]);
        }
        else
        {
            Debug.LogWarning("Audio index out of range: " + audioIndex);
        }
    }

    // UI ȿ���� ����
    public void PlayUISound(int audioIndex)
    {
        if (audioIndex >= 0 && audioIndex < audioClipsUI.Length)
        {
            source.PlayOneShot(audioClipsUI[audioIndex]);
        }
    }

}
