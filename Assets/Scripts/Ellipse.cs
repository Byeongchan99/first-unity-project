using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ellipse : MonoBehaviour
{
    public float xRadius, yRadius; // ����, ���� ������ 
    public Transform characterTransform; // Ÿ���� �߽�
    public bool isSafeZone; // ���� ���� ����
    public SpriteRenderer spriteRenderer;

    private Vector3 dot1, dot2; // Ÿ���� ���ǿ� �ش��ϴ� �� ���� F,F'

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Ÿ���� ���ǿ� �ش��ϴ� �� ���� ���Ѵ�.
        dot1 = characterTransform.position;
        dot1.x -= (float)Math.Sqrt(xRadius * xRadius - yRadius * yRadius);
        dot2 = characterTransform.position;
        dot2.x += (float)Math.Sqrt(xRadius * xRadius - yRadius * yRadius);

        spriteRenderer.enabled = false;
    }

    // ���ϴ� ���� ��ǥ�� Ÿ�� �ȿ� ���ִ��� Ȯ���Ѵ�.       
    public bool InEllipse(Transform other)
    {
        // �� �����κ����� �Ÿ��� ���� ���Ѵ�.
        float dist = 0;

        dist += Vector3.Distance(other.position, dot1);
        dist += Vector3.Distance(other.position, dot2);

        // 2x ���� ������ Ÿ���ȿ� ���Եȴ�.
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

            // Ÿ���� �ݽ��ϴ�.
            Gizmos.DrawLine(prevPos, startPos);
        }
    }
}
