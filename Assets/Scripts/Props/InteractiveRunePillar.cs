using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRunePillar : MonoBehaviour
{
    public GameObject firstPrefab;  // 처음 활성화되는 프리팹
    public GameObject secondPrefab;  // 상호작용 후 활성화되는 프리팹
    public int runeStageID;  // 룬의 ID
    public GameObject PortalSprite;

    void Start()
    {
        firstPrefab.SetActive(true);
        secondPrefab.SetActive(false);
    }

    void SwapPrefabs()
    {
        firstPrefab.SetActive(false);
        secondPrefab.SetActive(true);
    }

    void AcivePortalSprite()
    {
        PortalSprite.SetActive(true);
    }

    public void Interaction()
    {
        SwapPrefabs();
        StageManager.Instance.SetRuneStageCompleted(runeStageID, true);
        RuneGlowPillar.Instance.ActivateRune(runeStageID);
        UIManager.instance.mapRuneUI.ActivateRune(runeStageID);
        AcivePortalSprite();
    }
}
