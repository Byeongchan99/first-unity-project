using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : BaseAttackArea
{
    public GameObject trailParentObject; // ���ο� �θ� ������Ʈ ����
    private TrailRenderer trailRenderer;
    private List<Vector2> points = new List<Vector2>();

    void Awake()
    {
        if (trailParentObject)
            trailRenderer = trailParentObject.GetComponentInChildren<TrailRenderer>(); // �ڽ� ������Ʈ���� Ʈ���� �������� ã���ϴ�.

        if (trailRenderer)
            trailRenderer.enabled = false;
    }

    public override void ActivateAttackRange(Vector2 attackDirection, float weaponRange)
    {
        base.ActivateAttackRange(attackDirection, weaponRange);

        if (trailRenderer)
            trailRenderer.enabled = true;

        // Ʈ���� �θ� ������Ʈ ȸ��
        if (trailParentObject != null)
        {
            float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            trailParentObject.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    public override void CalculateColiderPoints(float radius)
    {
        points.Clear();
        int resolution = 30;

        points.Add(Vector2.zero);

        float startAngle = Mathf.PI / 18;
        float endAngle = 17 * Mathf.PI / 18;
        float deltaAngle = (endAngle - startAngle) / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            float t = startAngle + deltaAngle * i;
            points.Add(new Vector2(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius));
        }

        attackRangeCollider.SetPath(0, points);
        StartCoroutine(MoveTrailObject());
    }

    private IEnumerator MoveTrailObject()
    {
        foreach (Vector2 point in points)
        {
            if (trailRenderer)
                trailRenderer.transform.localPosition = point;
            yield return null;
        }

        if (trailRenderer)
            trailRenderer.enabled = false;
    }
}
