using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ellipse : MonoBehaviour
{
    public float xRadius, yRadius; // 가로, 세로 반지름 
    public Transform characterTransform; // 타원의 중심
    public bool isSafeZone; // 안전 구역 여부
    public SpriteRenderer spriteRenderer;

    private Vector3 dot1, dot2; // 타원의 정의에 해당하는 두 정점 F,F'

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 타원의 정의에 해당하는 두 점을 구한다.
        dot1 = characterTransform.position;
        dot1.x -= (float)Math.Sqrt(xRadius * xRadius - yRadius * yRadius);
        dot2 = characterTransform.position;
        dot2.x += (float)Math.Sqrt(xRadius * xRadius - yRadius * yRadius);

        spriteRenderer.enabled = false;
    }

    // 원하는 곳의 좌표가 타원 안에 들어가있는지 확인한다.       
    public bool InEllipse(Transform other)
    {
        // 두 점으로부터의 거리의 합을 구한다.
        float dist = 0;

        dist += Vector3.Distance(other.position, dot1);
        dist += Vector3.Distance(other.position, dot2);

        // 2x 보다 작으면 타원안에 포함된다.
        return dist <= xRadius * 2;
    }

    private void OnDrawGizmos()
    {
        if (characterTransform != null)
        {
            int segments = 360;
            float x;
            float y;

            Vector3 startPos = characterTransform.position + new Vector3(xRadius, 0, 0);
            Vector3 prevPos = startPos;

            for (int i = 0; i < segments; i++)
            {
                x = Mathf.Cos(Mathf.Deg2Rad * (i * 360f / segments)) * xRadius;
                y = Mathf.Sin(Mathf.Deg2Rad * (i * 360f / segments)) * yRadius;

                Vector3 newPos = characterTransform.position + new Vector3(x, y, 0);
                Gizmos.DrawLine(prevPos, newPos);
                prevPos = newPos;
            }

            // 타원을 닫습니다.
            Gizmos.DrawLine(prevPos, startPos);
        }
    }
}
