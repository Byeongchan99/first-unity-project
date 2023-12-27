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
        Debug.Log("attackID " + attackID);
        float angle = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        Debug.Log("콜라이더 활성화");
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

    // 몬스터 타격 이펙트
    IEnumerator DeactivateAfterSeconds(GameObject objectToDeactivate, float seconds)
    {
        yield return new WaitForSeconds(seconds); // 지정된 시간만큼 기다림
        objectToDeactivate.SetActive(false); // 객체 비활성화
    }

    public void DeactivateHitEffect(GameObject effect)
    {
        // 몬스터 타격 애니메이션 시간
        StartCoroutine(DeactivateAfterSeconds(effect, 0.417f));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 적에게 적중되었을 때
        if (other.CompareTag("Enemy"))
        {
            // Energy 회복
            PlayerStat.Instance.CurrentEnergy += 1;
            // Debug.Log("에너지: " + PlayerStat.Instance.CurrentEnergy);
            // 최대 Energy를 초과하는지 검사하고 초과 시 최대치로 설정
            PlayerStat.Instance.CurrentEnergy = Mathf.Min(PlayerStat.Instance.CurrentEnergy, PlayerStat.Instance.MaxEnergy);

            Debug.Log("이펙트 생성");
            // 충돌 위치를 계산
            //Vector3 hitPosition = other.transform.position;
            Vector3 hitPosition = other.ClosestPoint(transform.position);
            // 적중 위치에 타격 이펙트 활성화
            GameObject hitEffect = GameManager.instance.pool.Get(5);
            hitEffect.transform.position = hitPosition;
            // 타격 이펙트 회전
            Vector2 direction = (hitPosition - PlayerStat.Instance.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Debug.Log("angle:" + angle);
            hitEffect.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            DeactivateHitEffect(hitEffect);
        }
    }
}
