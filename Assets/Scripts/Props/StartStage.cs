using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage : MonoBehaviour
{
    public int transitionToStageIndex; // 이 포탈을 통해 이동할 스테이지의 인덱스

    // 다른 오브젝트가 이 포탈에 들어왔을 때 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("포탈 진입");
        if (collision.CompareTag("Player") && !GameManager.instance.isBattle)   // 전투 중이 아닐 때만
        {
            StageManager.Instance.TransitionToStage(transitionToStageIndex);
        }
    }
}
