using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSortingChanger : MonoBehaviour
{
    public string layerToChange; // 이 콜라이더에 접촉했을 때 변경할 Layer 이름
    public string sortingLayerToChange; // 변경할 Sorting Layer 이름

    private void ChangeLayerAndSorting(GameObject obj, string layerName, string sortingLayerName)
    {
        if (obj.name == "Shadow" || obj.name == "StandArea")
        {
            // Layer 변경
            obj.layer = LayerMask.NameToLayer(layerName);

            // Sorting Layer 변경 (Shadow에만 적용)
            if (obj.name == "Shadow")
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingLayerName = sortingLayerName;
                }
            }
        }

        // 모든 자식 오브젝트에 대해 재귀적으로 수행
        foreach (Transform child in obj.transform)
        {
            ChangeLayerAndSorting(child.gameObject, layerName, sortingLayerName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // "Player" 태그를 가진 오브젝트가 접촉했는지 확인
        {
            ChangeLayerAndSorting(collision.gameObject, layerToChange, sortingLayerToChange);
        }
    }
}

