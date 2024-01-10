using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("효과음")]
    public AudioClip rollSound;
    public AudioClip attackSound;
    public AudioClip walkSound;
    public AudioClip chargeSoundBow;
    public AudioClip chargeSoundMagic;
    public AudioClip hitSound;
    // ... 기타 필요한 효과음 ...

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRollSound()
    {
        if (rollSound != null)
        {
            audioSource.PlayOneShot(rollSound);
        }
    }

    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }
    }

    public void PlayWalkSound()
    {
        if (walkSound != null)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }

    public void PlayChargeSound(int weaponIdx)   // 무기 종류에 따라 차지 사운드가 달라짐
    {
        switch (weaponIdx)
        {
            case 10:
                audioSource.PlayOneShot(chargeSoundBow);
                break;

            case 11:
                audioSource.PlayOneShot(chargeSoundMagic);
                break;
        }
    }

    public void PlayHitSound()
    {
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

    // 사운드 조절 기능들
    // 페이드 아웃
    public void FadeOutSound(float fadeTime)
    {
        StartCoroutine(FadeOutCoroutine(fadeTime));
    }

    private IEnumerator FadeOutCoroutine(float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    // 볼륨 및 피치 제어
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }
}
