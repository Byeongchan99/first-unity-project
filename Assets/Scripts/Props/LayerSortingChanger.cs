using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSortingChanger : MonoBehaviour
{
    [Tooltip("�� �ݶ��̴��� �������� �� ������ Layer �̸�")]
    public string layerToChange;

    [Tooltip("������ Sorting Layer �̸�")]
    public string sortingLayerToChange;

    [Tooltip("��� ���������� �ƴ� ������ �̵��� �� true�� ����")]
    public bool MoveToNotMain;

    private void ChangeLayerAndSorting(GameObject obj, string layerName, string sortingLayerName)
    {
        if (obj.name == "Shadow" || obj.name == "StandArea" || obj.name == "Player")
        {
            // Layer ����
            obj.layer = LayerMask.NameToLayer(layerName);

            if (obj.name == "StandArea" && MoveToNotMain)   // ���� ���������� �ƴ� ������ StandArea�� ���� �ٲ��ֱ�
            {
                obj.layer = LayerMask.NameToLayer("PlayerStandArea");
            }

            // Sorting Layer ���� (Shadow�� Player ����)
            if (obj.name == "Shadow" || obj.name == "Player")
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingLayerName = sortingLayerName;
                }
            }
        }

        // ��� �ڽ� ������Ʈ�� ���� ��������� ����
        foreach (Transform child in obj.transform)
        {
            ChangeLayerAndSorting(child.gameObject, layerName, sortingLayerName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" �±׸� ���� ������Ʈ�� �����ߴ��� Ȯ��
        {
            ChangeLayerAndSorting(collision.gameObject, layerToChange, sortingLayerToChange);
        }
    }
}

