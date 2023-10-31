using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public int transitionToStageIndex; // �� ��Ż�� ���� �̵��� ���������� �ε���

    // �ٸ� ������Ʈ�� �� ��Ż�� ������ �� ȣ��Ǵ� �޼���
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

    // �ٸ� ������Ʈ�� �� ��Ż�� ����� �� ȣ��Ǵ� �޼���
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
