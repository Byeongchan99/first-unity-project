using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowTree : MonoBehaviour
{
    public int currentStageID; // 현재 스테이지 ID

    void Update()
    {
        currentStageID = StageManager.Instance.currentStage.stageID;

        // 현재 스테이지가 클리어되었는지 확인
        if (StageManager.Instance.IsStageCompleted(currentStageID))
        {
            // 클리어되었다면 이 오브젝트를 비활성화
            gameObject.SetActive(false);
        }
    }
}
