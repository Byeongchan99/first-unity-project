using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("ȿ����")]
    public AudioClip rollSound;
    public AudioClip attackSound;
    public AudioClip walkSound;
    public AudioClip chargeSoundBow;
    public AudioClip chargeSoundMagic;
    public AudioClip hitSound;
    // ... ��Ÿ �ʿ��� ȿ���� ...

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

    public void PlayChargeSound(int weaponIdx)   // ���� ������ ���� ���� ���尡 �޶���
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

    // ���� ���� ��ɵ�
    // ���̵� �ƿ�
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

    // ���� �� ��ġ ����
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }
}
