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

    // UIManager의 다른 메서드들...
    // 상점  UI
    public Shop shopUI;

    public void SetShopUI(Shop newShopUI)
    {
        shopUI = newShopUI;
    }

    // 일시정지 UI
    public PauseMenu pauseMenuUI;
}

