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
        // �ʱ� ���� ���� �����̴��� ����� �ҽ��� ����
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

    // Hide ��� OnExitButton() ���
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
        // �����̴��� ����� �� �ӽ� ���� ���� ������Ʈ
        soundEffectTempVolume = SoundEffectVolumeSlider.value;
        backGroundTempVolume = BackGroundVolumeSlider.value;
    }

    public void OnConfirmButton()
    {
        // ���� ��ư�� ������ �ӽ� ���� ���� ���� ����� �ҽ��� ����
        SoundEffectSource.volume = soundEffectTempVolume;
        BackGroundSource.volume = backGroundTempVolume;
    }

    public void OnExitButton()
    {
        // ��� ��ư�� ������ �����̴��� ���� ���� ������ �ǵ���
        SoundEffectVolumeSlider.value = SoundEffectSource.volume;
        BackGroundVolumeSlider.value = BackGroundSource.volume;
        rect.localScale = Vector3.zero;
        UIManager.instance.pauseMenuUI.isOpenedSoundSetting = false;
    }
}
