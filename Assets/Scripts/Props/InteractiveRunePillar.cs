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
        StageManager.Instance.SetRuneStageCompleted(runeStageID, true);   // 룬 스테이지 클리어 여부 설정
        RuneGlowPillar.Instance.ActivateRune(runeStageID);   // 메인 스테이지 중앙의 룬 기둥과 제단 룬 표시
        UIManager.instance.mapRuneUI.ActivateRune(runeStageID);   // 맵에 룬 표시
        UIManager.instance.shopUI.DisplayRandomShopItems();   // 상점 물품 초기화
        AcivePortalSprite();   // 포탈 스프라이트 표시
    }
}
