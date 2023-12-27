using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration = 0.1f; // 흔들림 지속 시간
    public float ShakeAmplitude = 1.2f; // 흔들림 강도
    public float ShakeFrequency = 1.0f; // 흔들림 빈도

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeElapsedTime = 0f;

    void Start()
    {
        // Cinemachine Virtual Camera 컴포넌트 찾기
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
        // 카메라 흔들기
        if (shakeElapsedTime > 0)
        {
            // Cinemachine Noise 컴포넌트 설정
            CinemachineBasicMultiChannelPerlin noise = cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = ShakeAmplitude;
            noise.m_FrequencyGain = ShakeFrequency;

            // 시간 감소
            shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            // 흔들기 끝
            CinemachineBasicMultiChannelPerlin noise = cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 0f;
            shakeElapsedTime = 0f;
        }
    }
}
