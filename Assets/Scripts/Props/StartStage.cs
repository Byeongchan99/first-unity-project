using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartStage : MonoBehaviour
{
    public int transitionToStageIndex; // �� ��Ż�� ���� �̵��� ���������� �ε���

    // �ٸ� ������Ʈ�� �� ��Ż�� ������ �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("��Ż ����");
        if (collision.CompareTag("Player") && !GameManager.instance.isBattle)   // ���� ���� �ƴ� ����
        {
            StageManager.Instance.TransitionToStage(transitionToStageIndex);
        }
    }
}
