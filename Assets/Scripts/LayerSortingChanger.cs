using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSortingChanger : MonoBehaviour
{
    public string layerToChange; // �� �ݶ��̴��� �������� �� ������ Layer �̸�
    public string sortingLayerToChange; // ������ Sorting Layer �̸�

    private void ChangeLayerAndSorting(GameObject obj, string layerName, string sortingLayerName)
    {
        if (obj.name == "Shadow" || obj.name == "StandArea")
        {
            // Layer ����
            obj.layer = LayerMask.NameToLayer(layerName);

            // Sorting Layer ���� (Shadow���� ����)
            if (obj.name == "Shadow")
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

