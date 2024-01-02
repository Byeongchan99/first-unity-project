using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingMenu : MonoBehaviour
{
    RectTransform rect;
    public Slider SoundEffectVolumeSlider, BackGroundVolumeSlider;
    public AudioSource SoundEffectSource, BackGroundSource;
    private float soundEffectTempVolume, backGroundTempVolume;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        // 초기 볼륨 값을 슬라이더와 오디오 소스에 설정
        soundEffectTempVolume = SoundEffectSource.volume;
        SoundEffectVolumeSlider.value = soundEffectTempVolume;

        backGroundTempVolume = BackGroundSource.volume;
        BackGroundVolumeSlider.value = backGroundTempVolume;
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        UIManager.instance.pauseMenuUI.isOpenedSoundSetting = true;
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // AudioManager.instance.EffectBgm(true);
    }

    // Hide 대신 OnExitButton() 사용
    /*
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }  
    */

    public void OnSliderChange()
    {
        // 슬라이더가 변경될 때 임시 볼륨 값을 업데이트
        soundEffectTempVolume = SoundEffectVolumeSlider.value;
        backGroundTempVolume = BackGroundVolumeSlider.value;
    }

    public void OnConfirmButton()
    {
        // 적용 버튼을 누르면 임시 볼륨 값을 실제 오디오 소스에 적용
        SoundEffectSource.volume = soundEffectTempVolume;
        BackGroundSource.volume = backGroundTempVolume;
    }

    public void OnExitButton()
    {
        // 취소 버튼을 누르면 슬라이더를 이전 볼륨 값으로 되돌림
        SoundEffectVolumeSlider.value = SoundEffectSource.volume;
        BackGroundVolumeSlider.value = BackGroundSource.volume;
        rect.localScale = Vector3.zero;
        UIManager.instance.pauseMenuUI.isOpenedSoundSetting = false;
    }
}
