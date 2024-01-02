using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource source;
    public AudioClip[] audioClips;   // 기타 효과음
    public AudioClip[] audioClipsUI;   // UI 효과음 - UI 버튼 클릭, 무기 장착, 맵 열기, 맵 닫기, 일시정지 메뉴 활성화, 일시정지 메뉴 비활성화, 업적 해금, 거부, 어빌리티 선택

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
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

    // 기타 효과음 실행
    public void PlaySound(int audioIndex)
    {
        Debug.Log("효과음 실행" + audioIndex);
        if (audioIndex >= 0 && audioIndex < audioClips.Length)
        {
            source.PlayOneShot(audioClips[audioIndex]);
        }
        else
        {
            Debug.LogWarning("Audio index out of range: " + audioIndex);
        }
    }

    // UI 효과음 실행
    public void PlayUISound(int audioIndex)
    {
        if (audioIndex >= 0 && audioIndex < audioClipsUI.Length)
        {
            source.PlayOneShot(audioClipsUI[audioIndex]);
        }
    }

}
