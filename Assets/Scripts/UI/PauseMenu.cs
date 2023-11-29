using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    RectTransform rect;
    public bool isPaused = false;   // 토글 플래그
    public bool isOpenedSoundSetting = false;   // 사운드 설정 플래그
    public bool isOpenedGameExitConfirm = false;   // 게임종료 확인 플래그

    // 게임종료 확인
    public GameObject gameExitConfirmPanel;
    // 사운드 설정
    public SoundSettingMenu soundSettingPanel;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }

    public void GameExitConfirm()
    {
        if (gameExitConfirmPanel != null)
        {
            gameExitConfirmPanel.SetActive(true);
            isOpenedGameExitConfirm = true;
        }
    }

    public void GameExitCancel()
    {
        if (gameExitConfirmPanel != null)
        {
            gameExitConfirmPanel.SetActive(false);
            isOpenedGameExitConfirm = false;
        }
    }

    // 게임종료. 전처리기를 이용해 에디터 아닐때 종료.
    public void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
