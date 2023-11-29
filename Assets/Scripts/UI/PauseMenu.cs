using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    RectTransform rect;
    public bool isPaused = false;   // ��� �÷���
    public bool isOpenedSoundSetting = false;   // ���� ���� �÷���
    public bool isOpenedGameExitConfirm = false;   // �������� Ȯ�� �÷���

    // �������� Ȯ��
    public GameObject gameExitConfirmPanel;
    // ���� ����
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

    // ��������. ��ó���⸦ �̿��� ������ �ƴҶ� ����.
    public void GameExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
