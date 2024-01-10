using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource source;
    public AudioClip[] audioClips;   // 기타 효과음 - 승리, 패배, 골드 사용, 체력 회복, 포탈, 수리 애니메이션, 업적창, 몬스터 소환
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
            float finalVolume = Mathf.Clamp(source.volume + volume, 0f, 1f); // 볼륨 범위를 0과 1 사이로 제한
            source.PlayOneShot(audioClips[audioIndex], finalVolume);
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
        if (audioIndex >= 0 && audioIndex < audioClipsUI.Length)
        {
            float finalVolume = Mathf.Clamp(source.volume + volume, 0f, 1f); // 볼륨 범위를 0과 1 사이로 제한
            source.PlayOneShot(audioClipsUI[audioIndex], finalVolume);
        }
    }
}
