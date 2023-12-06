using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowTree : MonoBehaviour
{
    public int currentStageID; // ���� �������� ID

    void Update()
    {
        currentStageID = StageManager.Instance.currentStage.stageID;

        // ���� ���������� Ŭ����Ǿ����� Ȯ��
        if (StageManager.Instance.IsStageCompleted(currentStageID))
        {
            // Ŭ����Ǿ��ٸ� �� ������Ʈ�� ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }
}
