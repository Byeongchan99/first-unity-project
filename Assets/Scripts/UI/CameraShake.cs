using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration = 0.1f; // ��鸲 ���� �ð�
    public float ShakeAmplitude = 1.2f; // ��鸲 ����
    public float ShakeFrequency = 1.0f; // ��鸲 ��

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeElapsedTime = 0f;

    void Start()
    {
        // Cinemachine Virtual Camera ������Ʈ ã��
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float duration, float amplitude, float frequency)
    {
        ShakeDuration = duration;
        ShakeAmplitude = amplitude;
        ShakeFrequency = frequency;
        shakeElapsedTime = ShakeDuration;
    }

    void Update()
    {
        // ī�޶� ����
        if (shakeElapsedTime > 0)
        {
            // Cinemachine Noise ������Ʈ ����
            CinemachineBasicMultiChannelPerlin noise = cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = ShakeAmplitude;
            noise.m_FrequencyGain = ShakeFrequency;

            // �ð� ����
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            // ���� ��
            CinemachineBasicMultiChannelPerlin noise = cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 0f;
            shakeElapsedTime = 0f;
        }
    }
}
