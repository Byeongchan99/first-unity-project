using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource source;
    public AudioClip[] audioClips;   // 기타 효과음 - 승리, 패배, 골드 사용, 체력 회복, 포탈, 수리 애니메이션, 업적창, 몬스터 소환
    public AudioClip[] audioClipsUI;   // UI 효과음 - UI 버튼 클릭, 무기 장착, 맵 열기, 맵 닫기, 일시정지 메뉴 활성화, 일시정지 메뉴 비활성화, 업적 해금, 거부, 어빌리티 선택

    // 인스펙터에서 설정할 추가 볼륨 값
    [SerializeField] private float additionalVolume = 0.5f;
    float originalVolume;

    // 인스펙터 Onclick 이벤트용 메서드
    public void PlayUISoundWithAdditionalVolume(int audioIndex)
    {
        PlayUISound(audioIndex, additionalVolume);
    }

    void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
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

    // 기타 효과음 실행
    public void PlaySound(int audioIndex)
    {
        //Debug.Log("효과음 실행" + audioIndex);
        if (audioIndex >= 0 && audioIndex < audioClips.Length)
        {
            source.PlayOneShot(audioClips[audioIndex]);
        }
        else
        {
            Debug.LogWarning("Audio index out of range: " + audioIndex);
        }
    }

    // 추가 볼륨으로 효과음 실행
    public void PlaySound(int audioIndex, float volume)
    {
        //Debug.Log("효과음 실행" + audioIndex);
        if (audioIndex >= 0 && audioIndex < audioClips.Length)
        {
            StartCoroutine(AdjustVolume(audioClips[audioIndex], volume));
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

    // 추가 볼륨으로 효과음 실행
    public void PlayUISound(int audioIndex, float volume)
    {
        Debug.Log("PlayUISound");
        if (audioIndex >= 0 && audioIndex < audioClipsUI.Length)
        {           
            StartCoroutine(AdjustVolume(audioClipsUI[audioIndex], volume));
        }
    }

    // 오디오 클립을 재생 후 원래 볼륨으로 변경
    private IEnumerator AdjustVolume(AudioClip audioClip, float volume)
    {
        Debug.Log("AdjustVolume");
        source.volume = Mathf.Clamp(source.volume + volume, 0f, 1f);   // 볼륨 범위를 0과 1 사이로 제한
        source.PlayOneShot(audioClip);
        yield return new WaitForSecondsRealtime(audioClip.length);
        source.volume = originalVolume;
        Debug.Log(source.volume);
    }
}
