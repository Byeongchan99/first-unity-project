using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : BaseAttackArea
{
    public GameObject trailParentObject; // 새로운 부모 오브젝트 참조
    public TrailRenderer trailRenderer;
    private List<Vector2> points = new List<Vector2>();

    private Vector2 currentAttackDirection;
    private float currentWeaponRange;

    void Awake()
    {
        if (trailRenderer)
            trailRenderer.enabled = false;
    }

    // 공격 범위 활성화
    public override void ActivateAttackRange(Vector2 attackDirection, float weaponRange)
    {
        attackID++;

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        attackRangeCollider.enabled = true;

        // 이전 트레일 데이터를 초기화
        if (trailRenderer)
        {
            float originalTime = trailRenderer.time;
            trailRenderer.time = 0;
            trailRenderer.Clear();
            trailRenderer.time = originalTime;
        }

        // 트레일 부모 오브젝트 회전
        if (trailParentObject != null)
        {
            // angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
            trailParentObject.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    // 콜라이더 모양 계산
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

    // 공격 이펙트 활성화
    public IEnumerator MoveTrailObject()
    {
        trailRenderer.enabled = true;

        // 첫번째 포인트(원의 중심)를 스킵하고 원호만 이동
        for (int i = 1; i < points.Count; i++)
        {
            if (trailRenderer)
                trailRenderer.transform.localPosition = points[i];
            yield return null;
        }

        if (trailRenderer)
        {
            trailRenderer.enabled = false;  // 트레일 렌더러 비활성화
        }
    }
}
