using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRunePillar : MonoBehaviour
{
    public GameObject firstPrefab;  // 처음 활성화되는 프리팹
    public GameObject secondPrefab;  // 상호작용 후 활성화되는 프리팹
    public GameObject questSprite;   // 퀘스트 스프라이트
    public int runeStageID;  // 룬의 ID
    public GameObject PortalSprite;
    public string text;   // 봉인석 복구 완료 메시지

    void Start()
    {
        questSprite.SetActive(true);
        firstPrefab.SetActive(true);
        secondPrefab.SetActive(false);
    }

    void SwapPrefabs()
    {
        questSprite.SetActive(false);
        firstPrefab.SetActive(false);
        secondPrefab.SetActive(true);
    }

    void AcivePortalSprite()
    {
        PortalSprite.SetActive(true);
    }

    public void Interaction()
    {
        StartCoroutine(WaitAndProcess());
    }

    IEnumerator WaitAndProcess()
    {
        UIManager.instance.repairAnimation.SetActive(true);   // 복구 애니메이션
        yield return new WaitForSeconds(3.0f); // 3초 동안 대기
        UIManager.instance.repairAnimation.SetActive(false);

        SwapPrefabs();   // 봉인석 교체
        StageManager.Instance.SetRuneStageCompleted(runeStageID, true);   // 룬 스테이지 클리어 여부 업데이트
        RuneGlowPillar.Instance.ActivateRune(runeStageID);   // 메인 스테이지 룬 활성화
        UIManager.instance.mapBackgroundUI.Show();   // 맵 활성화
        UIManager.instance.mapRuneUI.ActivateRune(runeStageID);   // 맵 룬 활성화
        UIManager.instance.shopUI.DisplayRandomShopItems();   // 상점 아이템 초기화

        yield return new WaitForSeconds(1.0f); // 4초 동안 대기(맵 활성화 후 룬 나타나는 시간 3초 + 1초 후 맵 비활성화)
        UIManager.instance.dialogueUI.Show(text);   // 봉인석 복구 메시지
        yield return new WaitForSeconds(3.0f); // 3초 동안 대기
        UIManager.instance.dialogueUI.Hide();
        PlayerController.Instance.isSystemNotice = false;
        AcivePortalSprite();
    }
}
