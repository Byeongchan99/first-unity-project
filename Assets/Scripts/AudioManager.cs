using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource source;
    public AudioClip[] audioClips;

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

    public void PlaySound(int audioIndex)
    {
        Debug.Log("���� ����" + audioIndex);
        if (audioIndex >= 0 && audioIndex < audioClips.Length)
        {
            source.PlayOneShot(audioClips[audioIndex]);
        }
        else
        {
            Debug.LogWarning("Audio index out of range: " + audioIndex);
        }
    }

}
