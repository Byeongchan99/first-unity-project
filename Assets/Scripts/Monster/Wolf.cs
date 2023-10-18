using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonsterBase
{
    public float waitTimeBeforeCharge = 2.0f; // 대기 시간
    public float chargeSpeed = 5.0f; // 돌진 속도
    public float chargeDuration = 2.0f; // 돌진하는 시간

    public override IEnumerator AttackPattern()
    {
        yield return new WaitForSeconds(waitTimeBeforeCharge); // 대기

        Vector2 chargeDirection = (target.position - transform.position).normalized; // 돌진 방향 설정

        float chargeEndTime = Time.time + chargeDuration; // 돌진을 시작하는 시간부터 돌진 지속 시간을 더해 돌진 종료 시간 설정

        // 돌진하는 동안 반복
        while (Time.time < chargeEndTime)
        {
            transform.position += (Vector3)chargeDirection * chargeSpeed * Time.deltaTime; // 돌진
            yield return null;
        }
    }
}
 