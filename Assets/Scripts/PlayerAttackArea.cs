using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : BaseAttackArea
{
    public override void CalculateColiderPoints(float radius)
    {
        List<Vector2> points = new List<Vector2>();
        int resolution = 30;

        // ���� �߽ɿ� ���� �� �߰�
        points.Add(Vector2.zero);

        // ���� ������ �� ������ �������� ����
        float startAngle = Mathf.PI / 18;    // 10 degrees in radians
        float endAngle = 17 * Mathf.PI / 18; // 170 degrees in radians

        float deltaAngle = (endAngle - startAngle) / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            float t = startAngle + deltaAngle * i;
            points.Add(new Vector2(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius));
        }

        attackRangeCollider.SetPath(0, points);
    }
}
