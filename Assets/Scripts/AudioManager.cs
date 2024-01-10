using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource source;
    public AudioClip[] audioClips;   // ��Ÿ ȿ���� - �¸�, �й�, ��� ���, ü�� ȸ��, ��Ż, ���� �ִϸ��̼�, ����â, ���� ��ȯ
    public AudioClip[] audioClipsUI;   // UI ȿ���� - UI ��ư Ŭ��, ���� ����, �� ����, �� �ݱ�, �Ͻ����� �޴� Ȱ��ȭ, �Ͻ����� �޴� ��Ȱ��ȭ, ���� �ر�, �ź�, �����Ƽ ����

    // �ν����Ϳ��� ������ �߰� ���� ��
    [SerializeField] private float additionalVolume = 0.5f;
    float originalVolume;

    // �ν����� Onclick �̺�Ʈ�� �޼���
    public void PlayUISoundWithAdditionalVolume(int audioIndex)
    {
        PlayUISound(audioIndex, additionalVolume);
    }

    void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� �ı����� �ʵ��� ����
            source = GetComponent<AudioSource>();
            originalVolume = source.volume;
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
        //Debug.Log("ȿ���� ����" + audioIndex);
        if (audioIndex >= 0 && audioIndex < audioClips.Length)
        {
            source.PlayOneShot(audioClips[audioIndex]);
        }
        else
        {
            Debug.LogWarning("Audio index out of range: " + audioIndex);
        }
    }

    // �߰� �������� ȿ���� ����
    public void PlaySound(int audioIndex, float volume)
    {
        //Debug.Log("ȿ���� ����" + audioIndex);
        if (audioIndex >= 0 && audioIndex < audioClips.Length)
        {
            StartCoroutine(AdjustVolume(audioClips[audioIndex], volume));
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

    // �߰� �������� ȿ���� ����
    public void PlayUISound(int audioIndex, float volume)
    {
        Debug.Log("PlayUISound");
        if (audioIndex >= 0 && audioIndex < audioClipsUI.Length)
        {           
            StartCoroutine(AdjustVolume(audioClipsUI[audioIndex], volume));
        }
    }

    // ����� Ŭ���� ��� �� ���� �������� ����
    private IEnumerator AdjustVolume(AudioClip audioClip, float volume)
    {
        Debug.Log("AdjustVolume");
        source.volume = Mathf.Clamp(source.volume + volume, 0f, 1f);   // ���� ������ 0�� 1 ���̷� ����
        source.PlayOneShot(audioClip);
        yield return new WaitForSecondsRealtime(audioClip.length);
        source.volume = originalVolume;
        Debug.Log(source.volume);
    }
}
