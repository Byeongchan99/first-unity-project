using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraSetter : MonoBehaviour
{
    public Canvas canvas; // �ν����Ϳ��� �Ҵ�
    public int sortingOrder = 11; // ���ϴ� ������ ����

    void Start()
    {
        // UI�� ī�޶� ã��
        Camera uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();

        // Canvas�� ī�޶� �Ҵ�
        if (uiCamera != null && canvas != null)
        {
            canvas.worldCamera = uiCamera;

            // Canvas�� sorting order ����
            //canvas.sortingOrder = sortingOrder;
        }
    }
}

