using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackArea : BaseAttackArea
{
    private List<Vector2> points = new List<Vector2>();

    public void Initialize()
    {
        float radius = PlayerStat.Instance.weaponManager.Weapon.AttackRange;
        CalculateColiderPoints(radius);
    }

    // 공격 범위 활성화
    public override void ActivateAttackRange(Vector2 attackDirection)
    {
        attackID++;

        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        attackRangeCollider.enabled = true;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        // 적에게 적중되었을 때
        if (other.CompareTag("Enemy"))
        {
            // Energy 회복
            PlayerStat.Instance.CurrentEnergy += 1;
            Debug.Log("에너지: " + PlayerStat.Instance.CurrentEnergy);
            // 최대 Energy를 초과하는지 검사하고 초과 시 최대치로 설정
            PlayerStat.Instance.CurrentEnergy = Mathf.Min(PlayerStat.Instance.CurrentEnergy, PlayerStat.Instance.MaxEnergy);
        }
    }
}
