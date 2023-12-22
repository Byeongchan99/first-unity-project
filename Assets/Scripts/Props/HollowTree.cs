using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowTree : MonoBehaviour
{
    public int currentStageID; // ���� �������� ID

    void Update()
    {
        if (StageManager.Instance != null)
        {
            currentStageID = StageManager.Instance.currentStage.stageID;

            if (StageManager.Instance.IsStageCompleted(currentStageID))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
