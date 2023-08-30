using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : BaseAttackArea
{
    public GameObject trailParentObject; // ���ο� �θ� ������Ʈ ����
    public TrailRenderer trailRenderer;
    private List<Vector2> points = new List<Vector2>();

    private Vector2 currentAttackDirection;
    private float currentWeaponRange;

    void Awake()
    {
        if (trailRenderer)
            trailRenderer.enabled = false;
    }

    // ���� ���� Ȱ��ȭ
    public override void ActivateAttackRange(Vector2 attackDirection, float weaponRange)
    {
        attackID++;

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        attackRangeCollider.enabled = true;

        // ���� Ʈ���� �����͸� �ʱ�ȭ
        if (trailRenderer)
        {
            float originalTime = trailRenderer.time;
            trailRenderer.time = 0;
            trailRenderer.Clear();
            trailRenderer.time = originalTime;
        }

        // Ʈ���� �θ� ������Ʈ ȸ��
        if (trailParentObject != null)
        {
            // angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            trailParentObject.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    // �ݶ��̴� ��� ���
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
    }

    // ���� ����Ʈ Ȱ��ȭ
    public IEnumerator MoveTrailObject()
    {
        trailRenderer.enabled = true;

        // ù��° ����Ʈ(���� �߽�)�� ��ŵ�ϰ� ��ȣ�� �̵�
        for (int i = 1; i < points.Count; i++)
        {
            if (trailRenderer)
                trailRenderer.transform.localPosition = points[i];
            yield return null;
        }

        if (trailRenderer)
        {
            trailRenderer.enabled = false;  // Ʈ���� ������ ��Ȱ��ȭ
        }
    }
}
