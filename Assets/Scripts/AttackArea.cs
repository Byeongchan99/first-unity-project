using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController;

public class AttackArea : MonoBehaviour
{
    public PlayerController playerController;
    public PolygonCollider2D attackRangeCollider;

    void Awake()
    {
        attackRangeCollider = GetComponent<PolygonCollider2D>();
    }

    void Start()
    {
        // 초기 상태에서는 공격 범위 콜라이더를 비활성화
        attackRangeCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected in attack area!");
        }
    }

    public void AttackRange()
    {
        float weaponRange = PlayerStat.Instance.weaponManager.Weapon.AttackRange;

        Vector2[] semiCirclePoints = CalculateSemiCirclePoints(weaponRange);
        attackRangeCollider.SetPath(0, semiCirclePoints);

        // attackDirection을 사용하여 AttackRangeObject를 회전시킵니다.
        float angle = Mathf.Atan2(playerController.attackDirection.y, playerController.attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);  // -90은 반원이 위쪽을 향하게 하기 위함입니다.

        attackRangeCollider.enabled = true;  // 공격 콜라이더 활성화
    }

    private Vector2[] CalculateSemiCirclePoints(float radius, int resolution = 30)
    {
        List<Vector2> points = new List<Vector2>();

        // 원의 중심에 대한 점 추가
        points.Add(Vector2.zero);

        // 시작 각도와 끝 각도를 라디안으로 설정
        float startAngle = Mathf.PI / 18;    // 10 degrees in radians
        float endAngle = 17 * Mathf.PI / 18; // 170 degrees in radians

        float deltaAngle = (endAngle - startAngle) / resolution;

        for (int i = 0; i <= resolution; i++)
        {
            float t = startAngle + deltaAngle * i;
            points.Add(new Vector2(Mathf.Cos(t) * radius, Mathf.Sin(t) * radius));
        }

        return points.ToArray();
    }

}
