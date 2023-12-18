using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRunePillar : MonoBehaviour
{
    public GameObject firstPrefab;  // ó�� Ȱ��ȭ�Ǵ� ������
    public GameObject secondPrefab;  // ��ȣ�ۿ� �� Ȱ��ȭ�Ǵ� ������
    public GameObject questSprite;   // ����Ʈ ��������Ʈ
    public int runeStageID;  // ���� ID
    public GameObject PortalSprite;
    public string text;   // ���μ� ���� �Ϸ� �޽���

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
        UIManager.instance.repairAnimation.SetActive(true);   // ���� �ִϸ��̼�
        yield return new WaitForSeconds(3.0f); // 3�� ���� ���
        UIManager.instance.repairAnimation.SetActive(false);

        SwapPrefabs();   // ���μ� ��ü
        StageManager.Instance.SetRuneStageCompleted(runeStageID, true);   // �� �������� Ŭ���� ���� ������Ʈ
        RuneGlowPillar.Instance.ActivateRune(runeStageID);   // ���� �������� �� Ȱ��ȭ
        UIManager.instance.mapBackgroundUI.Show();   // �� Ȱ��ȭ
        UIManager.instance.mapRuneUI.ActivateRune(runeStageID);   // �� �� Ȱ��ȭ
        UIManager.instance.shopUI.DisplayRandomShopItems();   // ���� ������ �ʱ�ȭ

        yield return new WaitForSeconds(1.0f); // 4�� ���� ���(�� Ȱ��ȭ �� �� ��Ÿ���� �ð� 3�� + 1�� �� �� ��Ȱ��ȭ)
        UIManager.instance.dialogueUI.Show(text);   // ���μ� ���� �޽���
        yield return new WaitForSeconds(3.0f); // 3�� ���� ���
        UIManager.instance.dialogueUI.Hide();
        PlayerController.Instance.isSystemNotice = false;
        AcivePortalSprite();
    }
}
