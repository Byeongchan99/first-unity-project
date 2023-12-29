using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("ȿ����")]
    public AudioClip rollSound;
    public AudioClip attackSound;
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
}
