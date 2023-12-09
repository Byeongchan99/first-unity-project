using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRunePillar : MonoBehaviour
{
    public GameObject firstPrefab;  // ó�� Ȱ��ȭ�Ǵ� ������
    public GameObject secondPrefab;  // ��ȣ�ۿ� �� Ȱ��ȭ�Ǵ� ������
    public int runeStageID;  // ���� ID
    public GameObject PortalSprite;

    void Start()
    {
        firstPrefab.SetActive(true);
        secondPrefab.SetActive(false);
    }

    void SwapPrefabs()
    {
        firstPrefab.SetActive(!firstPrefab.activeSelf);
        secondPrefab.SetActive(!secondPrefab.activeSelf);
    }

    void AcivePortalSprite()
    {
        PortalSprite.SetActive(true);
    }

    public void Interaction()
    {
        SwapPrefabs();
        StageManager.Instance.SetRuneStageCompleted(runeStageID, true);
        RuneGlowPillar.Instance.ActivatePillar(runeStageID);
        UIManager.instance.mapRuneUI.ActivateRune(runeStageID);
        AcivePortalSprite();
    }
}
