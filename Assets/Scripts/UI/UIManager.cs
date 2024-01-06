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

    // UIManager의 다른 메서드들...
    // 상점  UI
    public Shop shopUI;

    public void SetShopUI(Shop newShopUI)
    {
        shopUI = newShopUI;
    }

    // 일시정지 UI
    public PauseMenu pauseMenuUI;

    // 대화창 UI
    public Dialogue dialogueUI;

    /*
    public void SetDialogueUI(Dialogue newDialogueUI)
    {
        dialogueUI = newDialogueUI;
    }
    */

    // 맵 룬 효과
    public MapRune mapRuneUI;
    public MapBackground mapBackgroundUI;

    // 룬 복구 애니메이션
    public GameObject repairAnimation;

    // 게임오버 UI
    public GameObject gameOverUI;
    // 게임 승리 UI
    public GameObject gameVictoryUI;

    // 체력 UI
    public GameObject healthUI;
    // 에너지 UI
    public GameObject energyUI;
    // 현재 골드 UI
    public GameObject goldUI;
    // 현재 장비창 아이콘 UI
    public InventoryIcon InventoryIconUI;
}

