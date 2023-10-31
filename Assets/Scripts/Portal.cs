using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int transitionToStageIndex; // 이 포탈을 통해 이동할 스테이지의 인덱스

    // 다른 오브젝트가 이 포탈에 들어왔을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.SetNearPortal(true, transitionToStageIndex);
            }
        }
    }

    // 다른 오브젝트가 이 포탈을 벗어났을 때 호출되는 메서드
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.SetNearPortal(false, -1);
            }
        }
    }
}
