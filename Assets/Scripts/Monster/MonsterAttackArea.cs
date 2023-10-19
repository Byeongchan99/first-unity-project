using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterAttackArea : BaseAttackArea
{
    private Vector2 basePosition = new Vector2(0, 0.325f); // 초기 콜라이더 위치
    private MonsterBase monsterStat;

    void Awake()
    {
        monsterStat = GetComponentInParent<MonsterBase>();
    }

    void Start()
    {
        CalculateColiderPoints(monsterStat.attackRange); // 초기에 한 번만 호출
    }

    // 공격 범위 콜라이더 방향 계산 후 활성화
    public override void ActivateAttackRange(Vector2 attackDirection)
    {
        attackID++;

        // 방향 벡터를 4 방향 중 하나로 제한
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            attackDirection = attackDirection.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            attackDirection = attackDirection.y > 0 ? Vector2.up : Vector2.down;
        }

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // 콜라이더의 로컬 위치 조정
        Vector2 forwardOffset = attackDirection.normalized * monsterStat.attackColliderOffset;
        transform.localPosition = basePosition + forwardOffset; // 초기 위치에 이동 거리를 더함

        attackRangeCollider.enabled = true;
    }

    // 몬스터 공격 범위 계산
    public override void CalculateColiderPoints(float attackRange)
    {
        Vector2[] boxPoints = new Vector2[5];

        float halfWidth = attackRange / 2;
        float halfHeight = attackRange / 2;

        // 사각형의 꼭짓점 설정
        boxPoints[0] = new Vector2(-halfWidth, -halfHeight);  // 왼쪽 하단
        boxPoints[1] = new Vector2(halfWidth, -halfHeight);   // 오른쪽 하단
        boxPoints[2] = new Vector2(halfWidth, halfHeight);    // 오른쪽 상단
        boxPoints[3] = new Vector2(-halfWidth, halfHeight);   // 왼쪽 상단
        boxPoints[4] = boxPoints[0]; // 마지막 꼭짓점은 처음 꼭짓점과 같아야 폴리곤이 닫힌다.

        attackRangeCollider.SetPath(0, boxPoints);
    }
}
