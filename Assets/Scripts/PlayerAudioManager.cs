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
            case 0:
                audioSource.PlayOneShot(chargeSoundBow);
                break;

            case 1:
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
}
