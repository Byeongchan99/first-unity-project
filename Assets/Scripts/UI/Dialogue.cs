using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MerchantDialogue;

public class Dialogue : MonoBehaviour
{
    RectTransform rect;
    public Text dialogueText;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
       
        if (UIManager.instance != null)   // UI 매니저를 통해 참조
        {
            UIManager.instance.SetDialogueUI(this);
        }
        else
        {
            Debug.LogError("UIManager instance not found");
        }
    }

    public void Show(string dialogue)
    {
        rect.localScale = Vector3.one;
        dialogueText.text = dialogue;
        // GameManager.instance.Stop();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        // AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        // GameManager.instance.Resume();
        // AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        // AudioManager.instance.EffectBgm(false);
    }
}
