using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSortingChanger : MonoBehaviour
{
    [Tooltip("이 콜라이더에 접촉했을 때 변경할 Layer 이름")]
    public string layerToChange;

    [Tooltip("변경할 Sorting Layer 이름")]
    public string sortingLayerToChange;

    [Tooltip("허브 스테이지가 아닌 곳으로 이동할 때 true로 설정")]
    public bool MoveToNotMain;

    private void ChangeLayerAndSorting(GameObject obj, string layerName, string sortingLayerName)
    {
        if (obj.name == "Shadow" || obj.name == "StandArea" || obj.name == "Player")
        {
            // Layer 변경
            obj.layer = LayerMask.NameToLayer(layerName);

            if (obj.name == "StandArea" && MoveToNotMain)   // 메인 스테이지가 아닐 때에는 StandArea는 따로 바꿔주기
            {
                obj.layer = LayerMask.NameToLayer("PlayerStandArea");
            }

            // Sorting Layer 변경 (Shadow와 Player 적용)
            if (obj.name == "Shadow" || obj.name == "Player")
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

