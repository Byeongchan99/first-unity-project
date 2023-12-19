using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        instance = null;
    }

    // UIManager�� �ٸ� �޼����...
    // ����  UI
    public Shop shopUI;

    public void SetShopUI(Shop newShopUI)
    {
        shopUI = newShopUI;
    }

    // �Ͻ����� UI
    public PauseMenu pauseMenuUI;

    // ��ȭâ UI
    public Dialogue dialogueUI;

    /*
    public void SetDialogueUI(Dialogue newDialogueUI)
    {
        dialogueUI = newDialogueUI;
    }
    */

    // �� �� ȿ��
    public MapRune mapRuneUI;
    public MapBackground mapBackgroundUI;

    // �� ���� �ִϸ��̼�
    public GameObject repairAnimation;
}

