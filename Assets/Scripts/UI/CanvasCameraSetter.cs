using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraSetter : MonoBehaviour
{
    public Canvas canvas; // 인스펙터에서 할당
    public int sortingOrder = 11; // 원하는 순서로 설정

    void Start()
    {
        // UI용 카메라 찾기
        Camera uiCamera = GameObject.Find("UI Camera").GetComponent<Camera>();

        // Canvas에 카메라 할당
        if (uiCamera != null && canvas != null)
        {
            canvas.worldCamera = uiCamera;

            // Canvas의 sorting order 설정
            //canvas.sortingOrder = sortingOrder;
        }
    }
}

