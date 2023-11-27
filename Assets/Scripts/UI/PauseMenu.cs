using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    RectTransform rect;
    public bool isPaused = false;   // 토글 플래그

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
}
