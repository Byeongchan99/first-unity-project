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
        StageManager.Instance.SetRuneStageCompleted(runeStageID, true);   // �� �������� Ŭ���� ���� ����
        RuneGlowPillar.Instance.ActivateRune(runeStageID);   // ���� �������� �߾��� �� ��հ� ���� �� ǥ��
        UIManager.instance.mapRuneUI.ActivateRune(runeStageID);   // �ʿ� �� ǥ��
        UIManager.instance.shopUI.DisplayRandomShopItems();   // ���� ��ǰ �ʱ�ȭ
        AcivePortalSprite();   // ��Ż ��������Ʈ ǥ��
    }
}
